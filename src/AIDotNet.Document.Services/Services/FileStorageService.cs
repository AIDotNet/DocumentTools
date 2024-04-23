using System.Text;
using AIDotNet.Document.Services.Domain;

namespace AIDotNet.Document.Services.Services;

public sealed class FileStorageService(IFreeSql freeSql) : IFileStorageService
{
    public async ValueTask CreateOrUpdateFileAsync(string fileId, Stream stream)
    {
        // 先判断是否存在
        var file = freeSql.Select<FileStorageItem>().FirstAsync(x => x.Path == fileId);
        if (file != null)
        {
            await freeSql.Delete<FileStorageItem>().Where(x => x.Path == fileId)
                .ExecuteAffrowsAsync();
        }

        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);

        await freeSql.Insert<FileStorageItem>(new FileStorageItem()
        {
            Path = fileId,
            Content = memoryStream.ToArray(),
            Size = memoryStream.Length
        }).ExecuteAffrowsAsync();
    }

    public async Task CreateOrUpdateFileAsync(string fileId, string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        await CreateOrUpdateFileAsync(fileId, bytes);
    }

    public async Task CreateOrUpdateFileAsync(string fileId, byte[] bytes)
    {
        // 先判断是否存在
        var file = await freeSql.Select<FileStorageItem>().FirstAsync(x => x.Path == fileId);
        if (file != null)
        {
            await freeSql.Delete<FileStorageItem>().Where(x => x.Path == fileId)
                .ExecuteAffrowsAsync();
        }

        await freeSql.Insert(new FileStorageItem()
        {
            Path = fileId,
            Content = bytes,
            Size = bytes.Length
        }).ExecuteAffrowsAsync();
    }

    public async Task<string> CreateOrUpdateImageAsync(string fileId, string base64)
    {
        fileId = "https://image/" + fileId;
        // 先判断是否存在
        var file = freeSql.Select<FileStorageItem>().FirstAsync(x => x.Path == fileId);
        if (file != null)
        {
            await freeSql.Delete<FileStorageItem>().Where(x => x.Path == fileId)
                .ExecuteAffrowsAsync();
        }

        // 去掉base64头部
        base64 = base64.Substring(base64.IndexOf(',') + 1);
        var bytes = Convert.FromBase64String(base64);

        await freeSql.Insert(new FileStorageItem()
        {
            Path = fileId,
            Content = bytes,
            Size = bytes.Length
        }).ExecuteAffrowsAsync();

        return fileId;
    }

    public async Task<string> CreateOrUpdateImage(string fileId, string base64)
    {
        fileId = "https://image/" + fileId;
        var file = freeSql.Select<FileStorageItem>().FirstAsync(x => x.Path == fileId);
        if (file != null)
        {
            await freeSql.Delete<FileStorageItem>().Where(x => x.Path == fileId)
                .ExecuteAffrowsAsync();
        }

        // 去掉base64头部
        base64 = base64.Substring(base64.IndexOf(',') + 1);
        var bytes = Convert.FromBase64String(base64);

        await freeSql.Insert(new FileStorageItem()
        {
            Path = fileId,
            Content = bytes,
            Size = bytes.Length
        }).ExecuteAffrowsAsync();

        return fileId;
    }

    public async Task<string> GetFileContent(string fileId)
    {
        var file = await freeSql.Select<FileStorageItem>().Where(x => x.Path == fileId)
            .FirstAsync();
        if (file == null)
        {
            return string.Empty;
        }

        return Encoding.UTF8.GetString(file.Content);
    }

    public async ValueTask<byte[]> GetFileBytesAsync(string fileId)
    {
        var file = await freeSql.Select<FileStorageItem>().Where(x => x.Path == fileId)
            .FirstAsync();
        if (file == null)
        {
            return [];
        }

        return file.Content;
    }

    public async ValueTask<Stream> GetFileStreamAsync(string fileId)
    {
        var file = await freeSql.Select<FileStorageItem>().Where(x => x.Path == fileId)
            .FirstAsync();
        if (file == null)
        {
            return new MemoryStream();
        }

        return new MemoryStream(file.Content);
    }

    public async ValueTask RemoveFileAsync(string fileId)
    {
        await freeSql.Delete<FileStorageItem>().Where(x => x.Path == fileId)
            .ExecuteAffrowsAsync();
    }
}