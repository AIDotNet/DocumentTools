
namespace AIDotNet.Document.Rcl.Shared.Components.SideBars
{
    public partial class FullSideBar
    {
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
            refreshing = true;
            await OnRefreshClick.InvokeAsync();
            refreshing = false;
        }
    }
}
