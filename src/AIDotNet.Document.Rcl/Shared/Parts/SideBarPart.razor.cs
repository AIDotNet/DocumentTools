using AIDotNet.Document.Rcl.Shared.Components.SideBars;

namespace AIDotNet.Document.Rcl.Shared.Parts
{
    public partial class SideBarPart
    {
        [Parameter]
        public StringNumber FullWidth { get; set; } = 210;

        [Parameter]
        public bool Clipped { get; set; }

        [Parameter]
        public SideBarType SideBarType { get; set; } = SideBarType.Full;



        /// <summary>
        /// 客户端独有的刷新
        /// </summary>
        /// <returns></returns>
        async Task RefreshClick()
        {
            await Task.Delay(1000);
            await PopupService.ConfirmAsync("fake loading");
        }


        List<MenuItem> menus = [];

        protected override async Task OnInitializedAsync()
        {
            var menuItems = await MenuItemService.GetMenuItemsAsync();

            menus = menuItems.ToList();
        }
    }
}
