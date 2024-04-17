using System.Collections.Immutable;
using System.Text.Json;
using AIDotNet.Document.Services.Domain;

namespace AIDotNet.Document.Services.Services;

public class SettingService(IFreeSql freeSql) : ISettingService
{
    private static ImmutableList<Settings> Settings { get; set; } = ImmutableList<Settings>.Empty;


    public async ValueTask Update()
    {
        var settings = await freeSql.Select<Settings>().ToListAsync();
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
}