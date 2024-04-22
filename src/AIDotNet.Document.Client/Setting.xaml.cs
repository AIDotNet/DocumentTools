using System.Windows;
using AIDotNet.Document.Client.Services;
using AIDotNet.Document.Contract.Services;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Extensions.DependencyInjection;

namespace AIDotNet.Document.Client;

public partial class Setting : Window
{
    public Setting()
    {
        InitializeComponent();

        BlazorWeb.RootComponents.Add(new RootComponent()
        {
            Selector = "#app",
            ComponentType = typeof(Rcl.Components.Setting),
        });

        var app = ApplicationContext.BuildApplication();

        app.UseDocumentService();

        Resources.Add("services", app);
    }
}