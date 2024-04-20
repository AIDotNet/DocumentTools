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
                ComponentType = typeof(Document.App),
            });

            var app = services.BuildServiceProvider();
            
            app.UseDocumentService();

            Resources.Add("services", app);

            BlazorWeb.BlazorWebViewInitialized += (sender, args) =>
            {
                BlazorWeb.WebView.CoreWebView2.AddWebResourceRequestedFilter("https://image/*",
                    Microsoft.Web.WebView2.Core.CoreWebView2WebResourceContext.All);

                var fileStorageService = app.GetService<IFileStorageService>();

                BlazorWeb.WebView.CoreWebView2.WebResourceRequested += async (s, e) =>
                {
                    if (!e.Request.Uri.StartsWith("https://image/")) return;

                    var bytes = await fileStorageService!.GetFileBytesAsync(e.Request.Uri);

                    e.Response =
                        BlazorWeb.WebView.CoreWebView2.Environment.CreateWebResourceResponse(
                            new System.IO.MemoryStream(bytes), 200, "OK", "Content-Type: image/png");
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