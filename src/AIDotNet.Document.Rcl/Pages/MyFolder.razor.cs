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
    [SupplyParameterFromQuery(Name = "folderId")]
    public string? FolderId { get; set; }

    public FolderItemDto FolderItemDto { get; set; } = new();

    private List<FolderItemDto> Folders { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Folders = await FolderService.GetTreeFolderAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (FolderId != null)
        {
            FolderItemDto = Folders.First(x => x.Id == FolderId);

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
    }
    
    /// <summary>
    /// 新建笔记
    /// </summary>
    private void NewNote()
    {
        
    }

    /// <summary>
    /// 获取文件夹样式
    /// </summary>
    /// <param name="folderId"></param>
    /// <returns></returns>
    private string GetClass(string folderId)
        => folderId == FolderId ? "folder-item folder-item-selected" : "folder-item";
}