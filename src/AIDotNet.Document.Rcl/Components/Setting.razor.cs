using AIDotNet.Document.Contract;

namespace AIDotNet.Document.Rcl.Components;

public partial class Setting
{
    private OpenAIOptions _options = new();

    private List<ModelDto> _models;

    private List<EmbeddingDto> _embeddings;

    private string _version = string.Empty;

    protected override void OnInitialized()
    {
        // 获取当前程序集版本
        _version = typeof(Setting).Assembly.GetName().Version?.ToString();

        _embeddings = [new("text-embedding-3-large"), new("text-embedding-3-small"), new("text-embedding-ada-002"), new("text-embedding-v1"), new("text-moderation-latest"), new("text-moderation-stable"), new("text-search-ada-doc-001")];

        _models = GetDefModels();

        _options = SettingService.GetSetting<OpenAIOptions?>(Constant.Settings.OpenAIOptions) ?? new OpenAIOptions();
    }

    void SearchInputUpdate(string search)
    {
        if (search.Length > 0)
        {
            if (_models.Any(x => x.Name == search) is false)
            {
                _models = GetDefModels(search.ToLower());
            }
        }
        else
        {
            var def = GetDefModels();
            if (_models.Count != def.Count)
            {
                _models = def;
            }
        }
    }
    static List<ModelDto> GetDefModels(string key)
    {
        var models = GetDefModels();
        models.Insert(0, new(key));
        return models;
    }
    static List<ModelDto> GetDefModels() =>
        [
            new("gpt-3.5-turbo-0125"),
            new("gpt-3.5-turbo"),
            new("gpt-3.5-turbo-0301"),
            new("gpt-3.5-turbo-0613"),
            new("gpt-3.5-turbo-1106"),
            new("gpt-3.5-turbo-16k"),
            new("gpt-3.5-turbo-16k-0613"),
            new("gpt-3.5-turbo-instruct"),
            new("gpt-4"),
            new("gpt-4-0125-preview"),
            new("gpt-4-0314"),
            new("gpt-4-0613"),
            new("gpt-4-1106-preview"),
            new("gpt-4-turbo-2024-04-09"),
            new("gpt-4-32k"),
            new("gpt-4-32k-0314"),
            new("gpt-4-32k-0613"),
            new("gpt-4-all"),
            new("gpt-4-gizmo-*"),
            new("gpt-4-turbo-preview"),
            new("gpt-4-vision-preview")
        ];

    private async Task Save()
    {
        await SettingService.SetSetting(Constant.Settings.OpenAIOptions, _options);

        await PopupService.EnqueueSnackbarAsync("保存成功！", AlertTypes.Success);
    }
}
