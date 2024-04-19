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
            if (_item != null)
            {
                // 将异步转换为同步
                AsyncHelper.RunSync(async () => await SaveAsync());
            }

            _item = value;
            _ = LoadContentAsync();
        }
    }

    public bool IsEditName { get; set; }

    private string? Content { get; set; }

    [Parameter] public EventCallback<FolderItemDto> OnBlur { get; set; }

    private async Task Blur()
    {
        await OnBlur.InvokeAsync(Item);
    }
    
    protected override async Task OnInitializedAsync()
    {
        await LoadContentAsync();
    }

    public async ValueTask LoadContentAsync()
    {
        Content = await fileStorageService.GetFileContentAsync(Item.Id);
        await InvokeAsync(StateHasChanged);
    }

    public async ValueTask SaveAsync()
    {
        await fileStorageService.CreateOrUpdateFileAsync(Item.Id, Content ?? string.Empty);
    }

    public async ValueTask DisposeAsync()
    {
        await SaveAsync();
    }
}