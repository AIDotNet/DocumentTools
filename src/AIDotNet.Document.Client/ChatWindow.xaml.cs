﻿using System.Windows;
using AIDotNet.Document.Client.Services;
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
        };
        

        Loaded += (sender, e) =>
        {
            // 获取屏幕尺寸
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            // 设置窗口位置和大小
            Left = screenWidth - Width - 10;
            Top = screenHeight - Height - 50;

            // 设置窗口边框对齐屏幕角落
            WindowStyle = WindowStyle.None ;
            ResizeMode = ResizeMode.NoResize;
        };
    }
}