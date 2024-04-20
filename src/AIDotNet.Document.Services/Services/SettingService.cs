using System.Collections.Immutable;
using AIDotNet.Document.Services.Domain;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AIDotNet.Document.Services.Services;

public class SettingService(IFreeSql freeSql) : ISettingService
{
    private static ImmutableList<Settings> Settings { get; set; } = ImmutableList<Settings>.Empty;


    public void Update()
    {
        var settings = freeSql.Select<Settings>().ToList();
        Settings = settings.ToImmutableList();
    }

    public string GetSetting(string key)
    {
        return Settings.FirstOrDefault(x => x.Key == key)?.Value ?? string.Empty;
    }

    public T GetSetting<T>(string key)
    {
        var value = GetSetting(key);
        return string.IsNullOrEmpty(value) ? default : JsonSerializer.Deserialize<T>(value);
    }

    public int GetIntSetting(string key)
    {
        var value = GetSetting(key);
        return string.IsNullOrEmpty(value) ? 0 : int.TryParse(value, out var result) ? result : 0;
    }

    public bool GetBoolSetting(string key)
    {
        var value = GetSetting(key);
        return !string.IsNullOrEmpty(value) && (bool.TryParse(value, out var result) && result);
    }

    public async Task SetSetting(string key, string value)
    {
        var setting = Settings.FirstOrDefault(x => x.Key == key);
        if (setting != null)
        {
            setting.Value = value;
        }
        else
        {
            setting = new Settings
            {
                Key = key,
                Value = value
            };
            Settings = Settings.Add(setting);
        }
        
        // 判断是否存在
        if (freeSql.Select<Settings>().Where(x => x.Key == key).Count() > 0)
        {
            freeSql.Update<Settings>().Set(x => x.Value, value).Where(x => x.Key == key).ExecuteAffrows();
        }
        else
        {
            await freeSql.Insert(setting).ExecuteAffrowsAsync();
        }
    }

    public async Task SetSetting<T>(string key, T value)
    {
        await SetSetting(key, JsonSerializer.Serialize(value));
    }

    public async Task SetIntSetting(string key, int value)
    {
        await SetSetting(key, value.ToString());
    }

    public async Task SetBoolSetting(string key, bool value)
    {
        await SetSetting(key, value.ToString());
    }

    public void RemoveSetting(string key)
    {
        // 删除缓存
        Settings = Settings.Remove(Settings.FirstOrDefault(x => x.Key == key));
        // 删除数据库
        freeSql.Delete<Settings>().Where(x => x.Key == key).ExecuteAffrows();
    }
}