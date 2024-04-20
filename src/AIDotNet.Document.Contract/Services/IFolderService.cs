using AIDotNet.Document.Contract.Models;

namespace AIDotNet.Document.Contract.Services;

public interface IFolderService
{
    /// <summary>
    /// 获取文件夹
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<FolderItemDto> GetFolderByIdAsync(string id);
    
    /// <summary>
    /// 获取文件夹树
    /// </summary>
    /// <returns></returns>
    Task<List<FolderItemDto>> GetTreeFolderAsync();

    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    Task<string> CreateAsync(FolderItemDto folder);

    /// <summary>
    /// 删除文件夹
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RemoveAsync(string id);

    /// <summary>
    /// 修改文件夹 
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    Task UpdateAsync(FolderItemDto folder);
    
    /// <summary>
    /// 获取指定父目录下的所有子目录
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    Task<List<FolderItemDto>> GetFolderByParentIdAsync(string? parentId);
    
    /// <summary>
    /// 量化指定文件
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task QuantifyAsync(string id);
}