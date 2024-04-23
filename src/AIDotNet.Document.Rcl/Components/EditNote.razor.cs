using AIDotNet.Document.Contract.Services;
using AIDotNet.Infrastructure.Helpers;
using Masa.Blazor.Components.Editor;

namespace AIDotNet.Document.Rcl.Components;

public partial class EditNote : IAsyncDisposable
{
    [Inject] private IJSRuntime JS { get; set; }
    private DEditor DeditorRef { get; set; }

    private MMarkdown MMarkdownRef { get; set; }

    private Dictionary<string, object> _options = new();

    private FolderItemDto _value;

    private async Task<bool> BeforeAllUploadAsync(List<EditorUploadFileItem> flist)
    {
        await JS.InvokeVoidAsync("util.uploadFilePic", DeditorRef.ContentRef, DeditorRef.Ref, 0);
        return await Task.FromResult(true);
    }


    private async Task HandleUploadAsync()
    {
        await JS.InvokeVoidAsync("util.markdownUploadFile", MMarkdownRef.Ref, 0);
    }

    [Parameter]
    public FolderItemDto Value
    {
        get => _value;
        set
        {
            if (_value?.Id == value.Id)
            {
                return;
            }

            if (_value != null)
            {
                AsyncHelper.RunSync(Save);
            }

            _value = value;
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
        await OnBlur.InvokeAsync(Value);
    }

    protected override async Task OnInitializedAsync()
    {
        _options.Add("mode", "ir");
        await LoadContent();
    }

    public async Task LoadContent()
    {
        if (Value.Type == FolderType.Note || Value.Type == FolderType.Markdown)
        {
            Content = await fileStorageService.GetFileContent(Value.Id);
        }
        else
        {
            Content = Value.Id;
        }

        _ = InvokeAsync(StateHasChanged);
    }

    public async Task Save()
    {
        if (_isEditContent == true)
        {
            await fileStorageService.CreateOrUpdateFileAsync(Value.Id, Content ?? string.Empty);
        }
    }

    /// <summary>
    /// 向量
    /// </summary>
    private async Task Vector()
    {
        if (_value.Status == VectorStatus.Processing)
        {
            return;
        }

        // 防止重复点击
        await folderService.QuantifyAsync(_value.Id);

        _value.Status = VectorStatus.Processing;
    }

    public async ValueTask DisposeAsync()
    {
        await Save();

        await ValueTask.CompletedTask;
    }
}