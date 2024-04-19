namespace AIDotNet.Document.Contract.Services;

public interface IWindowService
{
    void Minimize();
    
    void Maximize();
    
    void Close();
    
    bool IsMaximized { get; }
}