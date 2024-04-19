namespace AIDotNet.Document.Rcl.Pages;

public partial class MyFolder
{
    /// <summary>
    /// 搜索关键字
    /// </summary>
    private string SearchKey { get; set; }

    /// <summary>
    /// 文件夹id
    /// </summary>
    [Parameter]
    public string? FolderId { get; set; }

    public FolderItemDto FolderItemDto { get; set; } = new();

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
    private void Back()
    {
        if (FolderItemDto.ParentId == null)
        {
            FolderId = null;
            return;
        }

        if (FolderItemDto.ParentId == null)
        {
            FolderId = null;
            return;
        }

        FolderItemDto = Folders.First(x => x.Id == FolderItemDto.ParentId);
        FolderId = FolderItemDto.Id;
    }

    /// <summary>
    /// 选择文件夹
    /// </summary>
    /// <param name="folderId"></param>
    public void SelectFolder(string folderId)
    {
        FolderItemDto = Folders.First(x => x.Id == folderId);
        FolderId = folderId;

        StateHasChanged();
    }

    /// <summary> 
    /// 新建笔记
    /// </summary>
    private async Task NewNote()
    {
        var id = FolderItemDto.IsFolder == true ? FolderItemDto.Id : FolderItemDto.ParentId;

        var item = new FolderItemDto()
        {
            Name = "无标题笔记",
            ParentId = id,
            IsFolder = false,
            Size = 0,
        };
        await FolderService.CreateAsync(item);

        Folders = await FolderService.GetFolderByParentIdAsync(id);
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