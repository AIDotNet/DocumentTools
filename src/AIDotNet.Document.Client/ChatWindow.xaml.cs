using System.Text.Json;
using System.Windows;
using AIDotNet.Document.Contract.Services;
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
            // 默认打开开发者工具
            webView.CoreWebView2.OpenDevToolsWindow();
            // 注册一个js调用的方法
            var chatService = app.GetService<IChatService>();

            // 等到前端postMessage的时候调用
            webView.WebMessageReceived += async (s, e) =>
            {
                // 解析协议：sendCallService: {}

                var value = e.TryGetWebMessageAsString();

                if (value.StartsWith("sendCallService:"))
                {
                    // 删除前面的sendCallService:
                    var json = e.WebMessageAsJson[16..];

                    CallService(json);

                    // 调用服务
                    void CallService(string json)
                    {
                        var service = JsonSerializer.Deserialize<SendCallServiceInput>(json);

                        // 找到指定的方法
                        var method = chatService.GetType().GetMethod(service.MethodName);

                        // 调用方法
                        var result = method?.Invoke(chatService, service.Parameters);

                        // 如果是异步方法，等待执行完成
                        if (method?.ReturnType == typeof(Task))
                        {
                            ((Task)result).Wait();
                        }
                        // 如果是 Task<T>
                        else if (method?.ReturnType.IsGenericType == true &&
                                 method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                        {
                            // 得到返回结果
                            var resultProperty = method.ReturnType.GetProperty("Result");
                            var resultValue = resultProperty?.GetValue(result);

                            // 等待执行完成
                            ((Task)resultValue).Wait();


                            // 发送结果
                            webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(
                                new CallServiceResultDto()
                                {
                                    EventId = service.EventId,
                                    Result = resultValue
                                }));
                        }
                        else if (method?.ReturnType == typeof(void))
                        {
                            // 发送结果
                            webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(
                                new CallServiceResultDto()
                                {
                                    EventId = service.EventId,
                                    Result = null
                                }));
                        }
                        else
                        {
                            // 如果是值类型，直接发送，否则序列化
                            if (result is ValueType)
                            {
                                webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(
                                    new CallServiceResultDto()
                                    {
                                        EventId = service.EventId,
                                        Result = result
                                    }));
                            }
                            else
                            {
                                webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(
                                    new CallServiceResultDto()
                                    {
                                        EventId = service.EventId,
                                        Result = JsonSerializer.Serialize(result)
                                    }));
                            }
                        }
                    }
                }
                else if (value.StartsWith("closeWindow"))
                {
                    CloseWindow();
                }
                else if (value.StartsWith("minimizeWindow"))
                {
                    MinimizeWindow();
                }
                else if (value.StartsWith("callService:"))
                {
                    // 删除前面的callService:
                    var json = value[12..];
                    CallService(json);

                    // 调用服务
                    async Task CallService(string json)
                    {
                        var service = JsonSerializer.Deserialize<SendCallServiceInput>(json, new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true,
                            // 可能是json字符串 存在 转移字符
                            ReadCommentHandling = JsonCommentHandling.Skip,
                            AllowTrailingCommas = true,
                        });

                        // 通过typename找到type
                        var type = Type.GetType(service.ServiceName);

                        // 通过type找到实例
                        var instance = app.GetService(type);

                        // 找到指定的方法
                        var method = type?.GetMethod(service.MethodName);

                        // 将service.Parameters转换为实际的类型
                        for (var i = 0; i < service.Parameters.Length; i++)
                        {
                            var str = service.Parameters[i].ToString();
                            var typeName = method?.GetParameters()[i].ParameterType;
                            service.Parameters[i] = JsonSerializer.Deserialize(str,
                                typeName, new JsonSerializerOptions()
                                {
                                    PropertyNameCaseInsensitive = true,
                                    // 可能是json字符串 存在 转移字符
                                    ReadCommentHandling = JsonCommentHandling.Skip,
                                    AllowTrailingCommas = true,
                                });
                        }

                        // 调用方法
                        var result = method?.Invoke(instance, service.Parameters);

                        // 如果是异步方法，等待执行完成
                        if (method?.ReturnType == typeof(Task))
                        {
                            ((Task)result).Wait();
                        }
                        // 如果是 Task<T>
                        else if (method?.ReturnType.IsGenericType == true &&
                                 method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                        {
                            // 得到返回结果
                            var resultProperty = method.ReturnType.GetProperty("Result");
                            var resultValue = resultProperty?.GetValue(result);

                            // 发送结果
                            webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(
                                new CallServiceResultDto()
                                {
                                    EventId = service.EventId,
                                    Result = resultValue
                                }));
                        }
                        else if (method?.ReturnType == typeof(void))
                        {
                            // 发送结果
                            webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(
                                new CallServiceResultDto()
                                {
                                    EventId = service.EventId,
                                    Result = null
                                }));
                        }
                        // 如果返回的是IAsyncEnumerable
                        else if (method?.ReturnType.IsGenericType == true &&
                                 method.ReturnType.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>))
                        {
                            // 循环等待并且发送
                            var enumerator = (IAsyncEnumerable<object>)result;

                            await foreach (var item in enumerator)
                            {
                                webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(
                                    new CallServiceResultDto()
                                    {
                                        EventId = service.EventId,
                                        Result = item,
                                        Type = "IAsyncEnumerable"
                                    }));
                            }

                            webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(
                                new CallServiceResultDto()
                                {
                                    EventId = service.EventId,
                                    Result = "IAsyncEnumerable:end",
                                    Type = "IAsyncEnumerable"
                                }));
                        }
                        else
                        {
                            // 如果是值类型，直接发送，否则序列化
                            if (result is ValueType)
                            {
                                webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(
                                    new CallServiceResultDto()
                                    {
                                        EventId = service.EventId,
                                        Result = result
                                    }));
                            }
                            else
                            {
                                webView.CoreWebView2.PostWebMessageAsString(JsonSerializer.Serialize(
                                    new CallServiceResultDto()
                                    {
                                        EventId = service.EventId,
                                        Result = JsonSerializer.Serialize(result)
                                    }));
                            }
                        }
                    }
                }
            };
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