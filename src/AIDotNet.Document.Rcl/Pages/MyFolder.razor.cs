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