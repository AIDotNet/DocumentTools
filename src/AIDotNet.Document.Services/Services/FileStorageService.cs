using System.Text;
using LiteDB;

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