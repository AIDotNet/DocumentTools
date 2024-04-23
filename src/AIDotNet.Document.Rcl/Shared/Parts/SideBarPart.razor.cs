namespace AIDotNet.Document.Rcl.Shared.Parts
{
    public partial class SideBarPart
    {
        [Parameter]
        public StringNumber Width { get; set; } = 210;


        bool refreshing;
        /// <summary>
        /// 客户端独有的刷新
        /// </summary>
        /// <returns></returns>
        async Task RefreshClick()
        {
            refreshing = true;
            await Task.Delay(1000);
            refreshing = false;
        }


        bool speedDialOpen;
    }
}
