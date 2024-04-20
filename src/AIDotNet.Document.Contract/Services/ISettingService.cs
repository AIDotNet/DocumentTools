namespace AIDotNet.Document.Contract.Services;

public interface ISettingService
{
    void Update();
    
    string GetSetting(string key);

    T GetSetting<T>(string key);

    int GetIntSetting(string key);
    
    bool GetBoolSetting(string key);
    
    Task SetSetting(string key, string value);
    
    Task SetSetting<T>(string key, T value);
    
    Task SetIntSetting(string key, int value);
    
    Task SetBoolSetting(string key, bool value);
    
    void RemoveSetting(string key);
    
}