namespace AIDotNet.Document.Contract.Services;

public interface ISettingService
{
    void Update();
    
    string GetSetting(string key);

    T GetSetting<T>(string key);

    int GetIntSetting(string key);
    
    bool GetBoolSetting(string key);
}