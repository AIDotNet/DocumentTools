namespace AIDotNet.Document.Contract.Models;

public sealed class FolderItemDto
{
    public string Id { get;  set; }

    public string Name { get;  set; }

    public bool IsEdit { get;  set; }

    public string? ParentId { get;  set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
    
    /// <summary>
    /// 是否目录
    /// </summary>
    public bool? IsFolder { get; set; }

    /// <summary>
    /// 如果是文件，文件大小
    /// </summary>
    public long Size { get; set; }

}
