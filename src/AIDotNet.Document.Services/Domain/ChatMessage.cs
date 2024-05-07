using FreeSql.DataAnnotations;

namespace AIDotNet.Document.Services.Domain;

public class ChatMessage
{
    public string? Content { get; set; }

    public DateTime CreateAt { get; set; }

    [Column(MapType = typeof(string), StringLength = -1)]
    public Dictionary<string, string> Extra { get; set; }

    [Column(IsPrimary = true)]
    public string? Id { get; set; }
    
    [Column(MapType = typeof(string), StringLength = -1)]
    public Dictionary<string, string> Meta { get; set; }

    public string? Role { get; set; }

    public DateTime? UpdateAt { get; set; }
}