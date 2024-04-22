using AIDotNet.Document.Contract.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AIDotNet.Document.Client;

public static class ApplicationContext
{
    private static IServiceCollection _services;

    public static IServiceProvider ServiceProvider { get; private set; }

    public static IServiceCollection CreateApplication()
    {
        if (_services != null)
            return _services;

        _services = new ServiceCollection();
        _services.AddDocumentRcl();
        _services.AddSingleton<IChatWindowService, ChatWindowService>();
        _services.AddWpfBlazorWebView();
        _services.AddDocumentService();
#if DEBUG
        _services.AddBlazorWebViewDeveloperTools();
#endif

        return _services;
    }

    public static IServiceProvider BuildApplication()
    {
        if (ServiceProvider != null)
        {
            return ServiceProvider;
        }

        return ServiceProvider = _services.BuildServiceProvider();
    }
}