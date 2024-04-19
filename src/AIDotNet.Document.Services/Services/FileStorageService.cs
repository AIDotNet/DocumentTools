using System.Text;

namespace AIDotNet.Document.Services.Services;

public sealed class FileStorageService(ILiteDatabase database) : IFileStorageService
{
    public ValueTask CreateOrUpdateFileAsync(string fileId, Stream stream)
    {
        // 先判断是否存在
        var file = database.FileStorage.FindById(fileId);
        if (file != null)
        {
            database.FileStorage.Delete(fileId);
        }

        database.FileStorage.Upload(fileId, fileId, stream);
        return ValueTask.CompletedTask;
    }

    public async ValueTask CreateOrUpdateFileAsync(string fileId, string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        await CreateOrUpdateFileAsync(fileId, bytes);
    }

    public ValueTask CreateOrUpdateFileAsync(string fileId, byte[] bytes)
    {
        // 先判断是否存在
        var file = database.FileStorage.FindById(fileId);
        if (file != null)
        {
            database.FileStorage.Delete(fileId);
        }

        database.FileStorage.Upload(fileId, fileId, new MemoryStream(bytes));
        return ValueTask.CompletedTask;
    }

    public async Task<string> CreateOrUpdateImageAsync(string name, string base64)
    {
        return await Task.Run(() =>
        {
            name = "https://image/" + name;
            var file = database.FileStorage.FindById(name);

            if (file != null)
            {
                database.FileStorage.Delete(name);
            }
            
            // 去掉base64头部
            base64 = base64.Substring(base64.IndexOf(',') + 1);

            var bytes = Convert.FromBase64String(base64);

            database.FileStorage.Upload(name, name, new MemoryStream(bytes));
            return name;
        });
    }

    public string CreateOrUpdateImage(string name, string base64)
    {
        name = "https://image/" + name;
        var file = database.FileStorage.FindById(name);

        if (file != null)
        {
            database.FileStorage.Delete(name);
        }

        var bytes = Convert.FromBase64String(base64);

        database.FileStorage.Upload(name, name, new MemoryStream(bytes));
        return name;
    }

    public ValueTask<string> GetFileContentAsync(string fileId)
    {
        var file = database.FileStorage.FindById(fileId);
        if (file == null)
        {
            return new ValueTask<string>(string.Empty);
        }

        using var stream = new MemoryStream();
        file.CopyTo(stream);
        return new ValueTask<string>(Encoding.UTF8.GetString(stream.ToArray()));
    }

    public ValueTask<byte[]> GetFileBytesAsync(string fileId)
    {
        var file = database.FileStorage.FindById(fileId);
        if (file == null)
        {
            return new ValueTask<byte[]>(Array.Empty<byte>());
        }

        using var stream = new MemoryStream();
        file.CopyTo(stream);
        return new ValueTask<byte[]>(stream.ToArray());
    }

    public ValueTask<Stream> GetFileStreamAsync(string fileId)
    {
        var file = database.FileStorage.FindById(fileId);
        if (file == null)
        {
            return new ValueTask<Stream>(Stream.Null);
        }

        return new ValueTask<Stream>(file.OpenRead());
    }

    public ValueTask RemoveFileAsync(string fileId)
    {
        database.FileStorage.Delete(fileId);
        return ValueTask.CompletedTask;
    }
}