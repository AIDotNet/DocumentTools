using FreeSql.DataAnnotations;

namespace AIDotNet.Document.Services.Domain;

[Table(Name = "settings")]
[Index("settings_key", "Key", true)]
public class Settings
{
    /// <summary>
    /// 主键
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 私有化（只有系统设置才显示）
    /// </summary>
    public bool Private { get; set; } = false;
}