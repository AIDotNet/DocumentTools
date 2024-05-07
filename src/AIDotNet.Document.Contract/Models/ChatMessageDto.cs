namespace AIDotNet.Document.Contract.Models;

public sealed class ChatMessageDto
{
    public string? Content { get; set; }

    public DateTime CreateAt { get; set; }

    public Dictionary<string, string> Extra { get; set; }

    public string? Id { get; set; }
    
    public Dictionary<string, string> Meta { get; set; }

    public string? Role { get; set; }

    public DateTime? UpdateAt { get; set; }
}

public class ChatMessageRole
{
    public const string Assistant = "assistant";

    public const string User = "user";

    public const string System = "system";

    public const string Tools = "tools";
}