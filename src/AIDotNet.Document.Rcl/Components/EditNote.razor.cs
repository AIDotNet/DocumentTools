namespace AIDotNet.Document.Rcl.Components;

public partial class EditNote
{
    [Parameter] public FolderItemDto Item { get; set; }

    private string? Content { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Content = await fileStorageService.GetFileContentAsync(Item.Id);
    }
}