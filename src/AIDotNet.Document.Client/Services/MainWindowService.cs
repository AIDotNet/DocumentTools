using System.Windows;
using AIDotNet.Document.Contract.Services;

namespace AIDotNet.Document.Client.Services;

public class MainWindowService(Window window) : IMainWindowService
{
    public void Minimize()
    {
        window.WindowState = WindowState.Minimized;
    }

    public void Maximize()
    {
        window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    public void Close()
    {
        //// 如果是MainWindow，则隐藏窗口
        //if (window is MainWindow mainWindow)
        //{
        //    mainWindow.Hide();

        //    return;
        //}
        // mainwindow处理

        window.Close();
    }

    public bool IsMaximized => window.WindowState == WindowState.Maximized;

    private Setting? _setting;

    public void OpenSetting(string type)
    {
        if(_setting != null)
        {
            _setting.Activate();
            return;
        }
        
        _setting = new Setting(type);
        _setting.Show();

        _setting.Closed += (sender, args) => { _setting = null; };

        _setting.Closing += (sender, args) =>
        {
            args.Cancel = true;
            _setting.Hide();
        };

        _setting.Activated += (sender, args) =>
        {
            _setting.Topmost = true;
            _setting.Topmost = false;
        };
    }

    public void Show()
    {
        window.Show();

        if (window.WindowState == WindowState.Minimized)
        {
            window.WindowState = WindowState.Normal;
        }

        window.Activate();
    }

    public void Hide()
    {
        window.Hide();
    }
}