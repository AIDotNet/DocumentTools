using AIDotNet.Document.Contract.Services;

namespace AIDotNet.Document.Client;

public class ChatWindowService : IChatWindowService
{
    private ChatWindow _chatWindow;

    public bool IsVisible { get; set; }

    public void Show(Action? onClosed = null)
    {
        if (_chatWindow != null)
        {
            _chatWindow.Close();
            _chatWindow = null;
        }

        _chatWindow = new ChatWindow();

        _chatWindow.Closed += (sender, args) =>
        {
            IsVisible = false;
            onClosed?.Invoke();
        };

        _chatWindow.Show();

        IsVisible = true;

        _chatWindow.Activate();
    }

    public void Hide()
    {
        if (_chatWindow != null)
        {
            _chatWindow.Hide();
            IsVisible = false;
        }
    }

    public void Close()
    {
        if (_chatWindow != null)
        {
            _chatWindow.Close();
            _chatWindow = null;
        }
    }
}