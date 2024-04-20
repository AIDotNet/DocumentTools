using System.Windows;
using AIDotNet.Document.Contract.Services;

namespace AIDotNet.Document.Client.Services;

public class WindowService(Window window) : IWindowService
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
        window.Close();
    }

    public bool IsMaximized => window.WindowState == WindowState.Maximized;

    private Setting? _setting;
    
    public void OpenSetting()
    {
        _setting = new Setting();
        _setting.Show();
        
        _setting.Closed += (sender, args) =>
        {
            _setting = null;
        };
        
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
}