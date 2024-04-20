namespace AIDotNet.Document.Contract.Services;

public interface IFileStorageService
{
    /// <summary>
    /// 存储文件
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="stream"></param>
    /// <returns></returns>
    ValueTask CreateOrUpdateFileAsync(string fileId, Stream stream);

    /// <summary>
    /// 存储文件
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    void CreateOrUpdateFileAsync(string fileId, string content);

    /// <summary>
    /// 存储文件
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    void CreateOrUpdateFileAsync(string fileId, byte[] bytes);

    /// <summary>
    /// 存储图片
    /// </summary>
    /// <param name="name"></param>
    /// <param name="base64"></param>
    /// <returns></returns>
    Task<string> CreateOrUpdateImageAsync(string name, string base64);

    /// <summary>
    /// 存储图片
    /// </summary>
    /// <param name="name"></param>
    /// <param name="base64"></param>
    /// <returns></returns>
    string CreateOrUpdateImage(string name, string base64);
    
    /// <summary>
    /// 获取文件内容
    /// </summary>
    /// <param name="fileId"></param>
    /// <returns></returns>
    string GetFileContent(string fileId);

    /// <summary>
    /// 获取文件字节
    /// </summary>
    /// <param name="fileId"></param>
    /// <returns></returns>
    ValueTask<byte[]> GetFileBytesAsync(string fileId);

    /// <summary>
    /// 获取文件流
    /// </summary>
    /// <param name="fileId"></param>
    /// <returns></returns>
    ValueTask<Stream> GetFileStreamAsync(string fileId);

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="fileId"></param>
    /// <returns></returns>
    ValueTask RemoveFileAsync(string fileId);
}