using FreeSql.DataAnnotations;

namespace AIDotNet.Document.Services.Domain;

[Table(Name = "file_storage.file_storage_item")]
public class FileStorageItem
{
    /// <summary>
    /// 具体路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 文件内容
    /// </summary>
    public byte[] Content { get; set; }

    /// <summary>
    /// 文件大小
    /// </summary>
    public long Size { get; set; }
}