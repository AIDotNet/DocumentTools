using System.Windows;
using AIDotNet.Document.Client.Services;
using AIDotNet.Document.Contract.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AIDotNet.Document.Client;

public partial class Setting : Window
{
    public Setting()
    {
        InitializeComponent();

        var services = new ServiceCollection();
        services.AddWpfBlazorWebView();
        services.AddDocumentRcl();
        services.AddDocumentService();
        services.AddSingleton<IWindowService>((_) => new WindowService(this));
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif

        BlazorWeb.RootComponents.Add(new Microsoft.AspNetCore.Components.WebView.Wpf.RootComponent()
        {
            Selector = "#app",
            ComponentType = typeof(Rcl.Components.Setting),
        });

        var app = services.BuildServiceProvider();

        app.UseDocumentService();

        Resources.Add("services", app);
    }
}