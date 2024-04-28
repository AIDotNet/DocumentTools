namespace AIDotNet.Document.Rcl.Pages;

public partial class Index
{
    private DataStatisticsDto _data = new();

    protected override async Task OnInitializedAsync()
    {
        _data = await DataStatisticsService.GetDataStatisticsAsync();
    }
}