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
}