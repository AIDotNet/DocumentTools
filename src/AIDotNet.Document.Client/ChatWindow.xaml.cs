using System.Windows;
using AIDotNet.Document.Contract.Services;
using AIDotNet.Document.Rcl.Components;
using AIDotNet.Document.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AIDotNet.Document.Client;

public partial class ChatWindow : Window
{
    public ChatWindow()
    {
        InitializeComponent();

        var app = ApplicationContext.BuildApplication();

        // BlazorWeb.RootComponents.Add(new Microsoft.AspNetCore.Components.WebView.Wpf.RootComponent()
        // {
        //     Selector = "#app",
        //     ComponentType = typeof(FloatingBallChat),
        // });
        //
        // app.UseDocumentService();
        //
        // Resources.Add("services", app);
        //
        // BlazorWeb.BlazorWebViewInitialized += (sender, args) =>
        // {
        //     BlazorWeb.WebView.CoreWebView2.AddWebResourceRequestedFilter("https://image/*",
        //         Microsoft.Web.WebView2.Core.CoreWebView2WebResourceContext.All);
        //
        //     var fileStorageService = app.GetService<IFileStorageService>();
        //
        //     BlazorWeb.WebView.CoreWebView2.WebResourceRequested += async (s, e) =>
        //     {
        //         if (!e.Request.Uri.StartsWith("https://image/")) return;
        //
        //         var bytes = await fileStorageService!.GetFileBytesAsync(e.Request.Uri);
        //
        //         e.Response =
        //             BlazorWeb.WebView.CoreWebView2.Environment.CreateWebResourceResponse(
        //                 new System.IO.MemoryStream(bytes), 200, "OK", "Content-Type: image/png");
        //     };
        //
        //     // 注册一个js调用的方法
        //     BlazorWeb.WebView.CoreWebView2.AddHostObjectToScript("fileStorageService", fileStorageService);
        //
        //     // 获取当前窗口句柄
        //     var windowHandle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
        //     var eventForwarder = new EventForwarder(windowHandle);
        //
        //     BlazorWeb.WebView.CoreWebView2.AddHostObjectToScript("eventForwarder", eventForwarder);
        //
        //     BlazorWeb.WebView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(
        //         "var fileStorageService = window.chrome.webview.hostObjects.fileStorageService;");
        //
        //     // 注册一个js调用的方法
        //     var chatService = app.GetService<IChatService>();
        //
        //     BlazorWeb.WebView.CoreWebView2.AddHostObjectToScript("chatService", chatService);
        //     
        //     BlazorWeb.WebView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(
        //         "var chatService = window.chrome.webview.hostObjects.chatService;");
        // };
        
        webView.CoreWebView2InitializationCompleted += (sender, e) =>
        {
            // 注册一个js调用的方法
            var freeSql = app.GetService<IFreeSql>();

            webView.CoreWebView2.AddHostObjectToScript("chatService", new ChatService(freeSql));
            
            webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(
                "var chatService = window.chrome.webview.hostObjects.chatService;");
            
            webView.CoreWebView2.AddHostObjectToScript("windowService", this);
            
            webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(
                "var windowService = window.chrome.webview.hostObjects.windowService;");
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
                    Width = 600;
                    Height = 700;
                    break;
                case > 1600:
                    Width = 580;
                    Height = 600;
                    break;
                default:
                    Width = 580;
                    Height = 580;
                    break;
            }

            // 设置窗口位置和大小
            Left = screenWidth - Width - 10;
            Top = screenHeight - Height - 50;


            // 置顶
            Topmost = true;
        };
    }
    
    public void CloseWindow()
    {
        Close();
    }
    
    public void MinimizeWindow()
    {
        WindowState = WindowState.Minimized;
    }
}