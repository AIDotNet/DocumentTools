using AIDotNet.Document.Contract.Services;

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
    /// 向量状态 | 文件夹不需要
    /// </summary>
    public VectorStatus Status { get; set; }
    
    public string StatusText => Status switch
    {
        VectorStatus.Unhandled => "未量化",
        VectorStatus.Processing => "量化中",
        VectorStatus.Failed => "量化失败",
        VectorStatus.Processed => "量化完成",
        _ => "未知"
    };
    
    /// <summary>
    /// 是否目录
    /// </summary>
    public bool? IsFolder { get; set; }

    /// <summary>
    /// 如果是文件，文件大小
    /// </summary>
    public long Size { get; set; }

}
