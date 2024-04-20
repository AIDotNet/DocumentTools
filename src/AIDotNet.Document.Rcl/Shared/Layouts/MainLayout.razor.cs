namespace AIDotNet.Document.Rcl.Shared.Layouts
{
    public partial class MainLayout
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // 等待1s
                await Task.Delay(400).ContinueWith(async _ =>
                {
                    await JSRuntime.InvokeVoidAsync("util.AILevitatedSphereInit");
                });
            }
        }
    }
}