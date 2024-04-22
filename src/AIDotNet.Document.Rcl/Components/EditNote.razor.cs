using AIDotNet.Document.Contract.Services;
using AIDotNet.Infrastructure.Helpers;
using Masa.Blazor.Components.Editor;

namespace AIDotNet.Document.Rcl.Components;

public partial class EditNote : IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; }
    private DEditor Ref { get; set; }

    private FolderItemDto _item;

    private async Task<bool> BeforeAllUploadAsync(List<EditorUploadFileItem> flist)
    {
        await JS.InvokeVoidAsync("util.uploadFilePic", Ref.ContentRef, Ref.Ref, 0);
        return await Task.FromResult(true);
    }

    [Parameter]
    public FolderItemDto Item
    {
        get => _item;
        set
        {
            if (_item?.Id == value.Id)
            {
                return;
            }

            if (_item != null)
            {
                AsyncHelper.RunSync(Save);
            }

            _item = value;
            AsyncHelper.RunSync(LoadContent);
        }
    }

    private bool IsEditName { get; set; }

    private bool _isEditContent;

    private string _content;

    private string? Content
    {
        get => _content;
        set
        {
            _isEditContent = true;
            _content = value;
        }
    }

    [Parameter] public EventCallback<FolderItemDto> OnBlur { get; set; }

    private async Task Blur()
    {
        await OnBlur.InvokeAsync(Item);
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadContent();
    }

    public async Task LoadContent()
    {
        Content = await fileStorageService.GetFileContent(Item.Id);
        _ = InvokeAsync(StateHasChanged);
    }

    public async Task Save()
    {
        if (_isEditContent == true)
        {
            await fileStorageService.CreateOrUpdateFileAsync(Item.Id, Content ?? string.Empty);
        }
    }

    /// <summary>
    /// 向量
    /// </summary>
    private async Task Vector()
    {
        if (_item.Status == VectorStatus.Processing)
        {
            return;
        }

        // 防止重复点击
        await folderService.QuantifyAsync(_item.Id);

        _item.Status = VectorStatus.Processing;
    }

    public async ValueTask DisposeAsync()
    {
        await Save();

        await ValueTask.CompletedTask;
    }
}