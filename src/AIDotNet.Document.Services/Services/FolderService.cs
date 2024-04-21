using System.Threading.Channels;
using AIDotNet.Document.Services.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace AIDotNet.Document.Services.Services;

public sealed class FolderService : IFolderService
{
    private readonly IFreeSql _freeSql;
    private Channel<Folder> FolderChannel { get; } = Channel.CreateUnbounded<Folder>();

    public FolderService(IFreeSql freeSql, IServiceProvider serviceProvider)
    {
        _freeSql = freeSql;
        Task.Run(async () =>
        {
            var values = await freeSql.Select<Folder>()
                .Where(x => x.Status == VectorStatus.Processing || x.Status == VectorStatus.Failed).ToListAsync();

            foreach (var value in values)
            {
                await FolderChannel.Writer.WriteAsync(value);
            }

            using var scope = serviceProvider.CreateScope();
            var fileStorageService = scope.ServiceProvider.GetRequiredService<IFileStorageService>();
            var sql = scope.ServiceProvider.GetRequiredService<IFreeSql>();
            while (await FolderChannel.Reader.WaitToReadAsync())
            {
                var folder = await FolderChannel.Reader.ReadAsync();
                var kernelMemory = scope.ServiceProvider.GetRequiredService<IKernelMemory>();
                await HandlerVectorAsync(folder, kernelMemory, fileStorageService, sql);
            }
        });
    }

    /// <summary>
    /// 处理向量内容
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="kernelMemory"></param>
    /// <param name="fileStorageService"></param>
    /// <param name="freeSql"></param>
    private async Task HandlerVectorAsync(Folder folder, IKernelMemory kernelMemory,
        IFileStorageService fileStorageService, IFreeSql freeSql)
    {
        if (folder.IsFolder == false)
        {
            try
            {
                var content = fileStorageService.GetFileContent(folder.Id);

                var tag = new TagCollection()
                {
                    {
                        "fileId", folder.Id
                    }
                };

                Console.WriteLine($"开始导入文件：{folder.Id}");

                await kernelMemory.ImportTextAsync(content, folder.Id, tag, index: "document");

                Console.WriteLine($"导入文件：{folder.Id} 完成");

                await freeSql.Update<Folder>()
                    .Set(f => f.Status, VectorStatus.Processed)
                    .Where(f => f.Id == folder.Id)
                    .ExecuteAffrowsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"导入文件：{folder.Id} 失败 {e.Message}");
                await freeSql.Update<Folder>()
                    .Set(f => f.Status, VectorStatus.Failed)
                    .Where(f => f.Id == folder.Id)
                    .ExecuteAffrowsAsync();
            }
        }
    }

    public async Task<FolderItemDto> GetFolderByIdAsync(string id)
    {
        var result = await _freeSql.Select<Folder>().Where(f => f.Id == id).FirstAsync();

        return new FolderItemDto()
        {
            Id = result.Id,
            Name = result.Name,
            ParentId = result.ParentId,
            Size = result.Size,
            IsFolder = result.IsFolder,
            CreatedTime = result.CreatedTime,
        };
    }

    public async Task<List<FolderItemDto>> GetTreeFolderAsync()
    {
        var rootFolders = await _freeSql.Select<Folder>().Where(f => f.ParentId == null && f.IsFolder).ToListAsync();

        return rootFolders.Select(x => new FolderItemDto
        {
            Id = x.Id,
            Name = x.Name,
            ParentId = x.ParentId
        }).ToList();
    }

    public async Task<string> CreateAsync(FolderItemDto folder)
    {
        if (folder.IsFolder == true)
        {
            var folderItem = new Folder(folder.Name, folder.ParentId);
            await _freeSql.Insert(folderItem)
                .ExecuteAffrowsAsync();

            return folderItem.Id;
        }
        else
        {
            var folderItem = new Folder(folder.Name, folder.ParentId, folder.Size);
            await _freeSql.Insert(folderItem)
                .ExecuteAffrowsAsync();

            return folderItem.Id;
        }
    }

    public async Task RemoveAsync(string id)
    {
        await _freeSql.Delete<Folder>().Where(f => f.Id == id).ExecuteAffrowsAsync();
    }

    public async Task UpdateAsync(FolderItemDto folder)
    {
        await _freeSql.Update<Folder>()
            .Where(f => f.Id == folder.Id)
            .Set(f => f.Name, folder.Name)
            .Set(x => x.ParentId, folder.ParentId)
            .Set(x => x.Size, folder.Size)
            .ExecuteAffrowsAsync();
    }

    public async Task<List<FolderItemDto>> GetFolderByParentIdAsync(string? parentId)
    {
        var folders = await _freeSql.Select<Folder>().Where(f => f.ParentId == parentId).ToListAsync();

        return folders.Select(x => new FolderItemDto
        {
            Id = x.Id,
            Name = x.Name,
            ParentId = x.ParentId,
            CreatedTime = x.CreatedTime,
            IsFolder = x.IsFolder,
        }).ToList();
    }

    public async Task QuantifyAsync(string id)
    {
        // 如果是文件夹，且状态不是处理中，才可以量化
        if (await _freeSql.Select<Folder>()
                .AnyAsync(x => x.Id == id && x.IsFolder == false && x.Status == VectorStatus.Processing))
        {
            return;
        }

        await _freeSql.Update<Folder>()
            .Set(f => f.Status, VectorStatus.Processing)
            .Where(f => f.Id == id)
            .ExecuteAffrowsAsync();

        var folder = await _freeSql.Select<Folder>().Where(f => f.Id == id).FirstAsync();

        await FolderChannel.Writer.WriteAsync(folder);
    }
}