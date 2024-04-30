
namespace AIDotNet.Document.Rcl.Shared.Components.SideBars
{
    /// <summary>
    /// 方案废弃
    /// </summary>
    public partial class FullSideBar
    {
        [Parameter]
        public SideBarType SideBarType { get; set; }

        [Parameter]
        public EventCallback<SideBarType> SideBarTypeChanged { get; set; }


        [Parameter, EditorRequired]
        public StringNumber Width { get; set; } = null!;

        [Parameter]
        public bool Clipped { get; set; }

        [Parameter, EditorRequired]
        public List<MenuItem> Menus { get; set; } = null!;

        [Parameter]
        public EventCallback OnRefreshClick { get; set; }

        bool refreshing;

        async Task RefreshClick()
        {
            temp = !temp;
            //refreshing = true;
            //await OnRefreshClick.InvokeAsync();
            //refreshing = false;
        }
    }
}
