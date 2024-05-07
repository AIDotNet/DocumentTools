namespace AIDotNet.Document.Contract.Models;

public class PageResult<T>(long total, IReadOnlyList<T> items)
{
    public long Total { get; set; } = total;

    public IReadOnlyList<T> Items { get; set; } = items;
}