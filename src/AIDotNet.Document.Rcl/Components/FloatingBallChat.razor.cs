using Microsoft.AspNetCore.Components.Web;

namespace AIDotNet.Document.Rcl.Components;

public partial class FloatingBallChat
{
    /// <summary>
    /// 开场白
    /// </summary>
    private const string Opening =
        """
        您好欢迎使用AIDotNet智能本地笔记助手，您可以提问您在当前服务器的所有笔记的内容，我会尽可能详细的回复您！
        """;

    private static readonly MarkdownItAnchorOptions s_anchorOptions = new()
    {
        Level = 1,
        PermalinkClass = "",
        PermalinkSymbol = ""
    };

    private string _id = Guid.NewGuid().ToString("N");

    private string _message = string.Empty;

    private bool _isLoading;

    private List<ChatMessageDto> ChatMessages { get; set; } = new();

    private void FloatingBall()
    {
        ChatWindowService.Close();
    }

    private async Task SendMessageAsync()
    {
        if (_message.IsNullOrWhiteSpace())
        {
            return;
        }

        ChatMessages.Add(new ChatMessageDto()
        {
            Content = _message,
            Role = ChatMessageRole.User,
            // CreateAt = DateTime.Now,
            // Extra = new Dictionary<string, string>(),
            // Id = Guid.NewGuid().ToString(),
            // Meta = new Dictionary<string, string>(),
        });

        _message = string.Empty;

        _ = InvokeAsync(StateHasChanged);

        var chat = new ChatMessageDto()
        {
            // CreateAt = DateTime.Now,
            // Extra = new Dictionary<string, string>(),
            // Id = Guid.NewGuid().ToString(),
            // Role = ChatMessageRole.Assistant,
            // Meta = new Dictionary<string, string>(),
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

                await JsRuntime.InvokeVoidAsync("util.scrollToBottom", _id);
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

    private void RemoveMessage(ChatMessageDto messageDto)
    {
        ChatMessages.Remove(messageDto);
    }

    private async Task CopyMessage(ChatMessageDto messageDto)
    {
        await JsRuntime.InvokeVoidAsync("util.copyToClipboard", messageDto.Content);

        await PopupService.EnqueueSnackbarAsync("复制成功", AlertTypes.Success);
    }

    private void HandleKeyPress(KeyboardEventArgs e)
    {
        if (e is { Key: "Enter", ShiftKey: false })
        {
            _ = SendMessageAsync();
        }
    }
    
    private void RemoveAllMessage()
    {
        ChatMessages.Clear();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // 等待1s
            await Task.Delay(600).ContinueWith(async _ =>
            {
                await JsRuntime.InvokeVoidAsync("util.AILevitatedSphereInit");
                await JsRuntime.InvokeVoidAsync("util.initTextEditor", "panel-textarea");
            });
        }
    }
}