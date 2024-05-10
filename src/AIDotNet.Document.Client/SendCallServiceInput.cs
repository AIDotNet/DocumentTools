namespace AIDotNet.Document.Client;

public class SendCallServiceInput
{
    public string EventId { get; set; }
    
    public string? ServiceName { get; set; }
    
    public string? MethodName { get; set; }
    
    public object?[]? Parameters { get; set; }
    
}

public class CallServiceResultDto
{
    public string EventId { get; set; }
    
    public object? Result { get; set; }

    /// <summary>
    /// 默认 Task | IAsyncEnumerable
    /// </summary>
    public string Type { get; set; } = "Task";
}