using AIDotNet.Document.Contract.Models;
using AIDotNet.Document.Services.Domain;

namespace AIDotNet.Document.Services.Services;

public sealed class FolderService(IFreeSql freeSql) : IFolderService
{
    public async Task<FolderItemDto> GetFolderByIdAsync(string id)
    {
        var result = await freeSql.Select<Folder>().Where(f => f.Id == id).FirstAsync();

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
        var rootFolders = await freeSql.Select<Folder>().Where(f => f.ParentId == null && f.IsFolder).ToListAsync();

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
            await freeSql.Insert(folderItem)
                .ExecuteAffrowsAsync();

            return folderItem.Id;
        }
        else
        {
            var folderItem = new Folder(folder.Name, folder.ParentId, folder.Size);
            await freeSql.Insert(folderItem)
                .ExecuteAffrowsAsync();

            return folderItem.Id;
        }
    }

    public async Task RemoveAsync(string id)
    {
        await freeSql.Delete<Folder>().Where(f => f.Id == id).ExecuteAffrowsAsync();
    }

    public async Task UpdateAsync(FolderItemDto folder)
    {
        await freeSql.Update<Folder>()
            .Where(f => f.Id == folder.Id)
            .Set(f => f.Name, folder.Name)
            .Set(x => x.ParentId, folder.ParentId)
            .Set(x => x.Size, folder.Size)
            .ExecuteAffrowsAsync();
    }

    public async Task<List<FolderItemDto>> GetFolderByParentIdAsync(string? parentId)
    {
        var folders = await freeSql.Select<Folder>().Where(f => f.ParentId == parentId).ToListAsync();

        return folders.Select(x => new FolderItemDto
        {
            Id = x.Id,
            Name = x.Name,
            ParentId = x.ParentId,
            CreatedTime = x.CreatedTime,
            IsFolder = x.IsFolder,
        }).ToList();
    }
}