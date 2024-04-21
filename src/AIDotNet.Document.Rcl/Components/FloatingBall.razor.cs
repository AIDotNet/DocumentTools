namespace AIDotNet.Document.Rcl.Components;

public partial class FloatingBall
{
    /// <summary>
    /// 显示浮动对话框
    /// </summary>
    private bool IsShowFloatingBall { get; set; }

    private string _id = Guid.NewGuid().ToString("N");

    private string _message = string.Empty;

    private bool _isLoading;

    private List<ChatMessage> ChatMessages { get; set; } = new();

    private async Task FloatingBallAsync()
    {
        IsShowFloatingBall = !IsShowFloatingBall;

        if (IsShowFloatingBall)
        {
            await Task.Delay(100).ContinueWith(async _ =>
            {
                await jsRuntime.InvokeVoidAsync("util.dragElement", "floating-panel");
            });
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task SendMessageAsync()
    {
        ChatMessages.Add(new ChatMessage()
        {
            Content = _message,
            Role = ChatMessageRole.User,
            CreateAt = DateTime.Now,
            Extra = new Dictionary<string, string>(),
            Id = Guid.NewGuid().ToString(),
            Meta = new Dictionary<string, string>(),
        });

        _message = string.Empty;

        _ = InvokeAsync(StateHasChanged);

        var chat = new ChatMessage()
        {
            CreateAt = DateTime.Now,
            Extra = new Dictionary<string, string>(),
            Id = Guid.NewGuid().ToString(),
            Role = ChatMessageRole.Assistant,
            Meta = new Dictionary<string, string>(),
        };

        // ChatMessages获取最后4条消息,但是要防止越界
        var history = ChatMessages.Skip(Math.Max(0, ChatMessages.Count - 4)).ToList();

        ChatMessages.Add(chat);

        try
        {
            _isLoading = true;
            await foreach (var item in KernelService.CompletionAsync(new CompletionInput()
                           {
                               History = history,
                               Relevancy = 0.5f
                           }))
            {
                chat.Content += item;
                _ = InvokeAsync(StateHasChanged);

                await jsRuntime.InvokeVoidAsync("util.scrollToBottom", _id);
            }
        }
        catch (Exception e)
        {
            chat.Content = e.Message;
        }
        finally
        {
            _isLoading = false;
        }

        await InvokeAsync(StateHasChanged);
    }

    private void RemoveMessage(ChatMessage message)
    {
        ChatMessages.Remove(message);
    }

    private async Task CopyMessage(ChatMessage message)
    {
        await jsRuntime.InvokeVoidAsync("util.copyToClipboard", message.Content);

        await PopupService.EnqueueSnackbarAsync("复制成功", AlertTypes.Success);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // 等待1s
            await Task.Delay(400).ContinueWith(async _ =>
            {
                await jsRuntime.InvokeVoidAsync("util.AILevitatedSphereInit");
            });
        }
    }
}