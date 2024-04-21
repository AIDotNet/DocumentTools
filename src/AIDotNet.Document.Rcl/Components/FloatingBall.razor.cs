namespace AIDotNet.Document.Rcl.Components;

public partial class FloatingBall
{
    /// <summary>
    /// 显示浮动对话框
    /// </summary>
    private bool IsShowFloatingBall { get; set; }

    private string Message = string.Empty;

    private List<ChatMessage> ChatMessages { get; set; } = new();

    private async Task FloatingBallAsync()
    {
        IsShowFloatingBall = !IsShowFloatingBall;

        await InvokeAsync(StateHasChanged);
    }

    private async Task SendMessageAsync()
    {
        ChatMessages.Add(new ChatMessage()
        {
            Content = Message,
            CreateAt = DateTime.Now,
            Extra = new Dictionary<string, string>(),
            Id = Guid.NewGuid().ToString(),
            Meta = new Dictionary<string, string>(),
        });

        Message = string.Empty;

        await InvokeAsync(StateHasChanged);

        var chat = new ChatMessage()
        {
            CreateAt = DateTime.Now,
            Extra = new Dictionary<string, string>(),
            Id = Guid.NewGuid().ToString(),
            Meta = new Dictionary<string, string>(),
        };

        // ChatMessages获取最后4条消息,但是要防止越界
        var history = ChatMessages.Skip(Math.Max(0, ChatMessages.Count - 4)).ToList();

        ChatMessages.Add(chat);

        await foreach (var item in KernelService.CompletionAsync(new CompletionInput()
                       {
                           History = history,
                           Relevancy = 0.5f
                       }))
        {
            chat.Content = item;
            ChatMessages.Add(chat);
            _ = InvokeAsync(StateHasChanged);
        }
    }

    protected override void OnInitialized()
    {
    }
}