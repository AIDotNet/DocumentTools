namespace AIDotNet.Document.Contract.Models;

public class ModelDto(string name)
{
    public string Name { get; set; } = name;

    public string Value { get; set; } = name;
}