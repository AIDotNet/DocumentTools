using AIDotNet.Document.Contract.Services;
using Microsoft.Win32;

namespace AIDotNet.Document.Client.Services;

public class FileService : IFileService
{
    public async Task OpenFileAsync(string filter, Action<string> callback)
    {
        await Task.Run(() =>
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = filter
            };

            if (openFileDialog.ShowDialog() == true)
            {
                callback(openFileDialog.FileName);
            }
        });
    }

    public async Task SaveFileAsync(string filter, Action<string> callback)
    {
        await Task.Run(() =>
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filter
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                callback(saveFileDialog.FileName);
            }
        });
    }
}