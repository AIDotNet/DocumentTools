namespace AIDotNet.Document.Rcl.Shared.Parts
{
    public partial class NavTreePart : ComponentBase
    {
        List<MenuItem> menus = [];

        protected override async Task OnInitializedAsync()
        {
            var menuItems = await MenuItemService.GetMenuItemsAsync();

            menus = menuItems.ToList();
        }
    }
}
