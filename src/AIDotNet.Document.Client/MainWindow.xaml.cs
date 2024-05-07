using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using AIDotNet.Document.Client.Services;
using AIDotNet.Document.Contract.Services;
using Serilog;

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

            // 创建Serilog
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Information()
#else
                .MinimumLevel.Warning()
#endif
                .WriteTo.File(
                    "./logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            Closing += MainWindow_Closing;

            var services = ApplicationContext.CreateApplication();

            services.AddSingleton<IMainWindowService>((_) => new MainWindowService(this));

            services.AddSerilog();
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

                // 监听word
                BlazorWeb.WebView.CoreWebView2.AddWebResourceRequestedFilter("https://word/*",
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
                    else if (e.Request.Uri.StartsWith("https://word/"))
                    {
                        var bytes = await fileStorageService!.GetFileBytesAsync(e.Request.Uri);

                        var cors = "Access-Control-Allow-Origin: *";
                        // 长度
                        cors += "\nContent-Length: " + bytes.Length;

                        e.Response = BlazorWeb.WebView.CoreWebView2.Environment.CreateWebResourceResponse(
                            new System.IO.MemoryStream(bytes), 200, "OK",
                            "Content-Type: application/msword\n" +
                            cors);
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

            RegisterEvents();
        }


        /// <summary>
        /// <para>从 settingpage 控制是否真的关闭</para>
        /// <para>warning 目前没做持久化 需要setting那边处理</para>
        /// </summary>
        public bool? TrueClose { get; set; }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (TrueClose is null)
            {
                var userSet = SetCloseOption();
                if (userSet is null)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    TrueClose = userSet;
                }
            }

            if (TrueClose is true)
            {
                return;
            }
            else if (TrueClose is false)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
            }
        }

        /// <summary>
        /// 以后美化 先running
        /// </summary>
        /// <returns></returns>
        static bool? SetCloseOption()
        {
            var content =
                "*DeBug开发模式* 设置关闭模式 设置后不在询问 设置类型如下:" +
                "\r\n\t是(Y)\t=>\t关闭应用" +
                "\r\n\t否(N)\t=>\t最小化到托盘" +
                "\r\n\t取消\t=>\t取消本次操作";

            var result = MessageBox.Show(content, "关闭设置", MessageBoxButton.YesNoCancel);
            return result switch
            {
                MessageBoxResult.Yes => true,
                MessageBoxResult.No => false,
                MessageBoxResult.Cancel => null,
                _ => null
            };
        }

        private void RegisterEvents()
        {
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException; //Task异常 

            //UI线程未捕获异常处理事件（UI主线程）
            this.Dispatcher.UnhandledException += App_DispatcherUnhandledException;

            //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                var exception = e.Exception as Exception;
                if (exception != null)
                {
                    HandleException(exception);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                e.SetObserved();
            }
        }

        //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)      
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    HandleException(exception);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //ignore
            }
        }

        //UI线程未捕获异常处理事件（UI主线程）
        private static void App_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                HandleException(e.Exception);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                e.Handled = true;
            }
        }

        private static void HandleException(Exception ex)
        {
            Log.Logger.Error(ex, "程序异常：{ex}", ex.Message);
        }
    }
}