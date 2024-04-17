namespace AIDotNet.Document.Rcl.JsInterop;

public sealed class CustomJsInterop(IJSRuntime js) : JSModule(js, "_content/AIDotNet.Document.Rcl/js/custom.js")
{
    public async ValueTask InitializeContextMenu(string id)
    {
        await InvokeVoidAsync("initializeContextMenu", id);
    }
    
}