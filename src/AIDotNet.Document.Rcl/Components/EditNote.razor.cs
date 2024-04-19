using AIDotNet.Infrastructure.Helpers;
using BlazorContextMenu;

namespace AIDotNet.Document.Rcl.Components;

public partial class EditNote : IAsyncDisposable
{
    private FolderItemDto _item;

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
    
    [Parameter]
    public EventCallback<FolderItemDto> OnBlur { get; set; }
    
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