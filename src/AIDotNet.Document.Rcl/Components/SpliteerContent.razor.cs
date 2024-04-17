
namespace AIDotNet.Document.Rcl.Components;

public partial class SpliteerContent
{
    [Parameter] public RenderFragment? Left { get; set; }

    [Parameter] public RenderFragment? Right { get; set; }

    [Parameter] public string Style { get; set; } = "height: 100%;";

    private string LeftStyle => "height: 100%;padding:5px;flex-direction: column;background-color:#FFFFFF!important;";

    private string RightStyle => "height: 100%;padding:5px;background-color:#FFFFFF!important;";
}