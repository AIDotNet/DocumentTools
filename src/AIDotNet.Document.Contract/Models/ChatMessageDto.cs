namespace AIDotNet.Document.Contract.Models;

public sealed class ChatMessageDto
{
    public string? Content { get; set; }

    public string? Role { get; set; }
}

public class ChatMessageRole
{
    public const string Assistant = "assistant";

    public const string User = "user";

    public const string System = "system";

    public const string Tools = "tools";
}