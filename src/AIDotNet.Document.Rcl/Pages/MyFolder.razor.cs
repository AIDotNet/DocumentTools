namespace AIDotNet.Document.Rcl.Pages;

public partial class MyFolder
{
    /// <summary>
    /// 文件夹id
    /// </summary>
    [Parameter]
    public string? FolderId { get; set; }

    /// <summary>
    /// 用于文件夹选择绑定
    /// </summary>
    public FolderItemDto? FolderItemDto { get; set; } = new();

    /// <summary>
    /// 用于笔记选择绑定
    /// </summary>
    public FolderItemDto FileItemDto { get; set; } = new();

    private List<FolderItemDto> Folders { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Folders = await FolderService.GetFolderByParentIdAsync(null);
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (FolderId != null)
        {
            FolderItemDto = await FolderService.GetFolderByIdAsync(FolderId);

            StateHasChanged();
        }
    }

    /// <summary>
    /// 返回上级目录
    /// </summary>
    private async Task Back()
    {
        if (FolderItemDto.ParentId == null)
        {
            FolderId = null;

            Folders = await FolderService.GetFolderByParentIdAsync(null);

            FolderItemDto = new FolderItemDto();

            return;
        }

        if (FolderItemDto.ParentId == null)
        {
            FolderId = null;
            return;
        }

        FolderItemDto = await FolderService.GetFolderByIdAsync(FolderItemDto.ParentId);

        Folders = await FolderService.GetFolderByParentIdAsync(FolderItemDto.ParentId);

        FolderId = FolderItemDto.Id;
    }

    /// <summary>
    /// 选择文件夹
    /// </summary>
    /// <param name="folderId"></param>
    public async Task SelectFolder(string folderId)
    {
        var item = Folders.First(x => x.Id == folderId);

        FolderId = folderId;

        if (item.IsFolder == false)
        {
            FileItemDto = item;
            return;
        }


        if (item.IsFolder == true)
        {
            Folders = await FolderService.GetFolderByParentIdAsync(folderId);
        }

        FileItemDto = new FolderItemDto();
        FolderItemDto = item;

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// 新建目录
    /// </summary>
    /// <param name="folderItemDto"></param>
    private async Task NewFolder(FolderItemDto folderItemDto)
    {
        if (folderItemDto.IsFolder == true)
        {
            var item = new FolderItemDto()
            {
                Name = "新建文件夹",
                ParentId = folderItemDto.Id,
                IsFolder = true,
            };
            await FolderService.CreateAsync(item);

            Folders = await FolderService.GetFolderByParentIdAsync(folderItemDto.Id);

            FolderItemDto = folderItemDto;
            FolderId = folderItemDto.Id;
        }
        else
        {
            var id = FolderItemDto?.IsFolder == true ? FolderItemDto.Id : FolderItemDto?.ParentId;

            var item = new FolderItemDto()
            {
                Name = "新建文件夹",
                ParentId = id,
                IsFolder = true,
            };
            await FolderService.CreateAsync(item);

            Folders = await FolderService.GetFolderByParentIdAsync(id);
        }
    }

    /// <summary> 
    /// 新建笔记
    /// </summary>
    /// <param name="folderItemDto"></param>
    private async Task NewNote(FolderItemDto? folderItemDto, FolderType type = FolderType.Note)
    {
        if (folderItemDto?.IsFolder == true)
        {
            var item = new FolderItemDto()
            {
                Name = "无标题笔记",
                ParentId = folderItemDto.Id,
                IsFolder = false,
                Type = type,
                Size = 0,
            };

            await FolderService.CreateAsync(item);

            Folders = await FolderService.GetFolderByParentIdAsync(folderItemDto.Id);

            FolderItemDto = folderItemDto;

            FolderId = folderItemDto.Id;
        }
        else
        {
            var id = FolderItemDto?.IsFolder == true ? FolderItemDto.Id : FolderItemDto?.ParentId;

            var item = new FolderItemDto()
            {
                Name = "无标题笔记",
                ParentId = id,
                Type = type,
                IsFolder = false,
                Size = 0,
            };
            await FolderService.CreateAsync(item);

            Folders = await FolderService.GetFolderByParentIdAsync(id);
        }
    }

    private async Task UploadWord(FolderItemDto? folderItemDto)
    {
        await FileService.OpenFileAsync(
            "Word Files (*.doc;*.docx)|*.doc;*.docx"
            , async path =>
            {
                // 限制一下文件大小
                var info = new FileInfo(path);

                if (info.Length > 1024 * 1024 * 10)
                {
                    await PopupService.EnqueueSnackbarAsync("文件大小不能超过10M", AlertTypes.Error);
                    return;
                }

                var parentId = folderItemDto?.IsFolder == true ? folderItemDto.Id : folderItemDto?.ParentId;

                var item = new FolderItemDto()
                {
                    Name = Path.GetFileName(path),
                    ParentId = parentId,
                    Type = FolderType.Word,
                    IsFolder = false,
                    Size = 0,
                };

                var id = await FolderService.CreateAsync(item);

                // TODO: 使用image协议让WebView2加载本地文件
                Folders = await FolderService.GetFolderByParentIdAsync(parentId);

                // 读取文件内容
                var bytes = await File.ReadAllBytesAsync(path);

                await FileStorageService.CreateOrUpdateFileAsync("https://word/" + id, bytes);

                FolderId = parentId;

                FolderItemDto = item;
            });
    }

    private async Task UploadPdf(FolderItemDto? folderItemDto)
    {
        await FileService.OpenFileAsync("PDF Files (*.pdf)|*.pdf", async path =>
        {
            // 限制一下文件大小
            var info = new FileInfo(path);

            if (info.Length > 1024 * 1024 * 10)
            {
                await PopupService.EnqueueSnackbarAsync("文件大小不能超过10M", AlertTypes.Error);
                return;
            }

            var parentId = folderItemDto?.IsFolder == true ? folderItemDto.Id : folderItemDto?.ParentId;

            var item = new FolderItemDto()
            {
                Name = Path.GetFileName(path),
                ParentId = parentId,
                Type = FolderType.Pdf,
                IsFolder = false,
                Size = 0,
            };

            var id = await FolderService.CreateAsync(item);

            // TODO: 使用image协议让WebView2加载本地文件
            Folders = await FolderService.GetFolderByParentIdAsync(parentId);

            // 读取文件内容
            var bytes = await File.ReadAllBytesAsync(path);

            await FileStorageService.CreateOrUpdateFileAsync("https://pdf/" + id, bytes);

            FolderId = parentId;

            FolderItemDto = item;
        });
    }

    /// <summary>
    /// 删除文件夹
    /// </summary>
    /// <param name="item"></param>
    private async Task Remove(FolderItemDto item)
    {
        await FolderService.RemoveAsync(item.Id);

        Folders.Remove(item);
    }

    private async Task Rename(FolderItemDto item)
    {
        item.IsEdit = true;
        await InvokeAsync(StateHasChanged);
    }

    private async Task ExportFile(FolderItemDto item)
    {
        await FileService.SaveFileAsync("", async path =>
        {
            var info = new FileInfo(path);


            switch (item.Type)
            {
                case FolderType.Markdown:

                    var bytes = await FileStorageService.GetFileBytesAsync(item.Id);

                    await File.WriteAllBytesAsync(Path.Combine(info.DirectoryName, item.Name + ".md"), bytes);

                    await PopupService.EnqueueSnackbarAsync("导出成功", AlertTypes.Success);
                    break;
                case FolderType.Note:

                    await File.WriteAllBytesAsync(Path.Combine(info.DirectoryName, item.Name + ".html"),
                        await FileStorageService.GetFileBytesAsync(item.Id));

                    break;
                case FolderType.Word:
                    var wordBytes = await FileStorageService.GetFileBytesAsync("https://word/" + item.Id);

                    await File.WriteAllBytesAsync(Path.Combine(info.DirectoryName, item.Name + ".docx"), wordBytes);
                    break;
                case FolderType.Pdf:
                    var pdfBytes = await FileStorageService.GetFileBytesAsync("https://pdf/" + item.Id);

                    await File.WriteAllBytesAsync(Path.Combine(info.DirectoryName, item.Name + ".pdf"), pdfBytes);
                    break;
            }

            await PopupService.EnqueueSnackbarAsync("导出成功", AlertTypes.Success);
        });
    }

    private async Task RenameOnBlur(FolderItemDto item)
    {
        item.IsEdit = false;
        await FolderService.UpdateAsync(item);
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// 获取文件夹样式
    /// </summary>
    /// <param name="folderId"></param>
    /// <returns></returns>
    private string GetClass(string folderId)
        => folderId == FolderId ? "folder-item folder-item-selected" : "folder-item";
}