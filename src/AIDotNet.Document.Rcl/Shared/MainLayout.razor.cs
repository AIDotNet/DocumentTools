using AIDotNet.Document.Contract.Models;
using BlazorComponent;

namespace AIDotNet.Document.Rcl.Shared;

public partial class MainLayout
{
    private List<FolderItemDto> folderItems = new();

    private StringNumber Width = "210px";

    private StringNumber _selectedItem;

    private bool _myFolderExpanded = false;

    private string tableContextMenu = Guid.NewGuid().ToString("N");

    public bool MyFolderExpanded
    {
        get => _myFolderExpanded;

        set
        {
            if (value && _myFolderExpanded != value)
            {
                NavigationManager.NavigateTo("/my-folder");
            }

            _myFolderExpanded = value;
        }
    }

    private async Task OnBlurAsync(FolderItemDto itemDto)
    {
        itemDto.IsEdit = false;
        await FolderService.UpdateAsync(itemDto);
        await InvokeAsync(StateHasChanged);
    }

    private async Task NewFolder(string? parentId)
    {
        MyFolderExpanded = true;

        if (parentId == null)
        {
            var folder = new FolderItemDto
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = "新建文件夹",
                IsEdit = true,
                ParentId = parentId
            };

            folderItems.Add(folder);
            await FolderService.CreateAsync(folder);
        }
        else
        {
            await FolderService.CreateAsync(new FolderItemDto()
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = "新建文件夹",
                IsEdit = true,
                ParentId = parentId
            });
        }
    }

    private async ValueTask LoadAsync()
    {
        folderItems = await FolderService.GetTreeFolderAsync();

        await InvokeAsync(StateHasChanged);
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task RemoveAsync(string id)
    {
        folderItems.Remove(folderItems.First(x => x.Id == id));

        await FolderService.RemoveAsync(id);

        await LoadAsync();
    }

    private void UpdateName(FolderItemDto item)
    {
        item.IsEdit = true;

        InvokeAsync(StateHasChanged);
    }
    
    private void SelectItem(FolderItemDto item)
    {
        _selectedItem = item.Id;
        
        NavigationManager.NavigateTo($"/my-folder?folderId={item.Id}");
    }
}