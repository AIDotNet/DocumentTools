using AIDotNet.Document.Contract.Models;
using AIDotNet.Document.Services.Domain;

namespace AIDotNet.Document.Services.Services;

public sealed class FolderService(IFreeSql freeSql) : IFolderService
{
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

    public async Task CreateAsync(FolderItemDto folder)
    {
        if (folder.IsFolder)
        {
            await freeSql.Insert(new Folder(folder.Name, folder.ParentId))
                .ExecuteAffrowsAsync();
        }
        else
        {
            await freeSql.Insert(new Folder(folder.Name, folder.ParentId, folder.Size))
                .ExecuteAffrowsAsync();
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
            ParentId = x.ParentId
        }).ToList();
    }
}