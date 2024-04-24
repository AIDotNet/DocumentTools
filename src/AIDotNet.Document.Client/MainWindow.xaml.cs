using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using AIDotNet.Document.Client.Services;
using AIDotNet.Document.Contract.Services;

namespace AIDotNet.Document.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var services = ApplicationContext.CreateApplication();

            services.AddSingleton<IMainWindowService>((_) => new MainWindowService(this));

            BlazorWeb.RootComponents.Add(new Microsoft.AspNetCore.Components.WebView.Wpf.RootComponent()
            {
                Selector = "#app",
                ComponentType = typeof(Document.App),
            });

            var app = ApplicationContext.BuildApplication();

            app.UseDocumentService();

            Resources.Add("services", app);

            BlazorWeb.BlazorWebViewInitialized += (sender, args) =>
            {
                BlazorWeb.WebView.CoreWebView2.AddWebResourceRequestedFilter("https://image/*",
                    Microsoft.Web.WebView2.Core.CoreWebView2WebResourceContext.All);

                // 监听pdf
                BlazorWeb.WebView.CoreWebView2.AddWebResourceRequestedFilter("https://pdf/*",
                    Microsoft.Web.WebView2.Core.CoreWebView2WebResourceContext.All);

                // 监听 */api/v1/upload
                BlazorWeb.WebView.CoreWebView2.AddWebResourceRequestedFilter("*/api/v1/upload",
                    Microsoft.Web.WebView2.Core.CoreWebView2WebResourceContext.All);

                var fileStorageService = app.GetService<IFileStorageService>();

                BlazorWeb.WebView.CoreWebView2.WebResourceRequested += async (s, e) =>
                {
                    if (e.Request.Uri.StartsWith("https://image/"))
                    {
                        var bytes = await fileStorageService!.GetFileBytesAsync(e.Request.Uri);

                        e.Response =
                            BlazorWeb.WebView.CoreWebView2.Environment.CreateWebResourceResponse(
                                new System.IO.MemoryStream(bytes), 200, "OK", "Content-Type: image/png");
                    }
                    else if (e.Request.Uri.StartsWith("https://pdf/"))
                    {
                        var bytes = await fileStorageService!.GetFileBytesAsync(e.Request.Uri);

                        e.Response = BlazorWeb.WebView.CoreWebView2.Environment.CreateWebResourceResponse(
                            new System.IO.MemoryStream(bytes), 200, "OK", "Content-Type: application/pdf");
                    }
                    else if (e.Request.Uri.Contains("/api/v1/upload"))
                    {
                        
                    }
                };

                // 注册一个js调用的方法
                BlazorWeb.WebView.CoreWebView2.AddHostObjectToScript("fileStorageService", fileStorageService);

                // 获取当前窗口句柄
                var windowHandle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
                var eventForwarder = new EventForwarder(windowHandle);

                BlazorWeb.WebView.CoreWebView2.AddHostObjectToScript("eventForwarder", eventForwarder);

                BlazorWeb.WebView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(
                    "var fileStorageService = window.chrome.webview.hostObjects.fileStorageService;");
            };
        }
    }
}