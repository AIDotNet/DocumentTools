using System.Configuration;
using System.Data;
using System.Windows;

namespace AIDotNet.Document.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var notifyIcon = new System.Windows.Forms.NotifyIcon()
            {
                Icon = new System.Drawing.Icon("logo.ico"),
                Visible = true,
                Text = "智能本地笔记助手",
            };

            notifyIcon.Click += (sender, args) =>
            {
                if (MainWindow == null)
                {
                    MainWindow = new MainWindow();
                    MainWindow.Show();
                }
                else
                {
                    MainWindow.Activate();
                }
            };


            notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

            notifyIcon.ContextMenuStrip.Items.Add("打开", null, (sender, args) =>
            {
                if (MainWindow == null)
                {
                    MainWindow = new MainWindow();
                    MainWindow.Show();
                }
                else
                {
                    MainWindow.Show();
                    
                    MainWindow.Activate();
                }
            });

            notifyIcon.ContextMenuStrip.Items.Add("退出", null, (sender, args) => { Shutdown(); });
        }
    }
}