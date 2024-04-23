namespace AIDotNet.Document.Contract.Services;

public interface IFileService
{
    /// <summary>
    /// 打开文件
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    Task OpenFileAsync(string filter,Action<string> callback);
}