namespace AIDotNet.Document.Contract.Models;

public class OpenAIOptions
{
    /// <summary>
    /// 向量化模型
    /// </summary>
    public string EmbeddingModel { get; set; } = string.Empty;

    /// <summary>
    /// 对话模型
    /// </summary>
    public string ChatModel { get; set; } = string.Empty;

    /// <summary>
    /// 端点
    /// </summary>
    public string Endpoint { get; set; }

    /// <summary>
    /// ApiKey
    /// </summary>
    public string ApiKey { get; set; }
}