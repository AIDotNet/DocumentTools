namespace AIDotNet.Document.Contract.Services;

public interface IMainWindowService
{
    void Minimize();
    
    void Maximize();
    
    void Close();
    
    bool IsMaximized { get; }
    
    /// <summary>
    /// 打开设置
    /// </summary>
    void OpenSetting(string type);
    
    void Show();
    
    void Hide();
}