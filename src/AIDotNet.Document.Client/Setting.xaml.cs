using System.Windows;
using AIDotNet.Document.Client.Services;
using AIDotNet.Document.Contract.Services;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Extensions.DependencyInjection;

namespace AIDotNet.Document.Client;

public partial class Setting : Window
{
    public Setting(string type)
    {
        InitializeComponent();

        BlazorWeb.RootComponents.Add(new RootComponent()
        {
            Selector = "#app",
            ComponentType = typeof(Rcl.Components.Setting),
            Parameters = new Dictionary<string, object?>()
            {
                { "type", type }
            }
        });

        var app = ApplicationContext.BuildApplication();

        app.UseDocumentService();

        Resources.Add("services", app);
    }
}