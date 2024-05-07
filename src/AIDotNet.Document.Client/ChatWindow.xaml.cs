using System.Windows;
using AIDotNet.Document.Contract.Services;
using AIDotNet.Document.Rcl.Components;
using Microsoft.Extensions.DependencyInjection;

namespace AIDotNet.Document.Client;

public partial class ChatWindow : Window
{
    public ChatWindow()
    {
        InitializeComponent();

        var app = ApplicationContext.BuildApplication();

        BlazorWeb.RootComponents.Add(new Microsoft.AspNetCore.Components.WebView.Wpf.RootComponent()
        {
            Selector = "#app",
            ComponentType = typeof(FloatingBallChat),
        });

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

            // 注册一个js调用的方法
            var chatService = app.GetService<IChatService>();

            BlazorWeb.WebView.CoreWebView2.AddHostObjectToScript("chatService", chatService);
            
            BlazorWeb.WebView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(
                "var chatService = window.chrome.webview.hostObjects.chatService;");
        };


        Loaded += (sender, e) =>
        {
            // 获取屏幕尺寸
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;


            switch (screenWidth)
            {
                // 根据当前屏幕尺寸设置窗口大小 当屏幕是4k的时候
                case > 1920:
                    Width = 400;
                    Height = 800;
                    break;
                case > 1600:
                    Width = 300;
                    Height = 600;
                    break;
                default:
                    Width = 200;
                    Height = 400;
                    break;
            }

            // 设置窗口位置和大小
            Left = screenWidth - Width - 10;
            Top = screenHeight - Height - 50;


            // 置顶
            Topmost = true;
        };
    }
}