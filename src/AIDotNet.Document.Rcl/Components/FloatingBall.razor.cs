namespace AIDotNet.Document.Rcl.Components;

public partial class FloatingBall
{
    /// <summary>
    /// 显示浮动对话框
    /// </summary>
    private bool IsShowFloatingBall { get; set; }

    private string _id = Guid.NewGuid().ToString("N");

    private async Task FloatingBallAsync()
    {
        mainWindowService.Hide();
        IsShowFloatingBall = true;

        chatWindowService.Show(() =>
        {
            IsShowFloatingBall = false;

            mainWindowService.Show();
            _ = InvokeAsync(StateHasChanged);
        });

        await InvokeAsync(StateHasChanged);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // 等待1s
            await Task.Delay(400).ContinueWith(async _ =>
            {
                await jsRuntime.InvokeVoidAsync("util.AILevitatedSphereInit");
            });
        }
    }
}