namespace AIDotNet.Document.Contract.Services;

public interface IChatWindowService
{
    bool IsVisible { get; set; }
    
    /// <summary>
    /// 显示聊天窗口
    /// </summary>
    /// <param name="onClosed">窗口关闭事件</param>
    void Show(Action? onClosed = null);
 
    void Hide();
    
    void Close();
}