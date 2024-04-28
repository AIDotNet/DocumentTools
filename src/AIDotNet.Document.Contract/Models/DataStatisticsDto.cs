namespace AIDotNet.Document.Contract.Models;

public class DataStatisticsDto
{
    /// <summary>
    /// 最近使用的笔记
    /// </summary>
    public FolderItemDto LatestUsageNote { get; set; }
    
    /// <summary>
    /// 总笔记数
    /// </summary>
    public long TotalNoteCount { get; set; }
    
    /// <summary>
    /// 最近编辑的笔记
    /// </summary>
    public FolderItemDto LatestUpdateNote { get; set; }
}