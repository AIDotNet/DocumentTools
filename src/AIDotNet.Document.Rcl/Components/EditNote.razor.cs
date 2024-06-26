﻿using AIDotNet.Document.Contract.Services;
using AIDotNet.Infrastructure.Helpers;
using Masa.Blazor.Components.Editor;

namespace AIDotNet.Document.Rcl.Components;

public partial class EditNote : IAsyncDisposable
{
    private DEditor DeditorRef { get; set; }

    private DMarkdown MMarkdownRef { get; set; }

    private Dictionary<string, object> _options = new();

    private FolderItemDto _value;

    private async Task<bool> BeforeAllUploadAsync(List<EditorUploadFileItem> flist)
    {
        await JsRuntime.InvokeVoidAsync("util.uploadFilePic", DeditorRef.ContentRef, DeditorRef.Ref, 0);
        return await Task.FromResult(true);
    }


    private async Task HandleUploadAsync()
    {
        await JsRuntime.InvokeVoidAsync("util.markdownUploadFile", MMarkdownRef.Ref, 0);
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
        if (Value.Type is FolderType.Note or FolderType.Markdown)
        {
            Content = await FileStorageService.GetFileContent(Value.Id);
        }
        else
        {
            Content = Value.Id;
        }

        if (Value.Type == FolderType.Word)
        {
            Task.Run((async () =>
            {
                await Task.Delay(100);
                
                await Js.InvokeVoidAsync("util.loadDocs", "https://word/" + Content, "load-docs");
            }));
        }

        _ = InvokeAsync(StateHasChanged);
    }

    public async Task Save()
    {
        if (_isEditContent == true)
        {
            await FileStorageService.CreateOrUpdateFileAsync(Value.Id, Content ?? string.Empty);
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
        await FolderService.QuantifyAsync(_value.Id);

        _value.Status = VectorStatus.Processing;
    }

    public async ValueTask DisposeAsync()
    {
        await Save();

        await ValueTask.CompletedTask;
    }
}