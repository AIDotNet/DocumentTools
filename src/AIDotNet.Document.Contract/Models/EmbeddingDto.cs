namespace AIDotNet.Document.Contract.Models;

public class EmbeddingDto(string name)
{
    public string Name { get; set; } = name;

    public string Value { get; set; } = name;
}