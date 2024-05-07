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
        WindowService.Hide();
        IsShowFloatingBall = true;

        ChatWindowService.Show(() =>
        {
            IsShowFloatingBall = false;

            WindowService.Show();
            _ = InvokeAsync(StateHasChanged);
        });

        await InvokeAsync(StateHasChanged);
    }

}