namespace AIDotNet.Document.Contract.Models;

public class OpenAIOptions
{
    /// <summary>
    /// 向量化模型
    /// </summary>
    public string EmbeddingModel { get; set; } = "text-embedding-3-large";

    /// <summary>
    /// 对话模型
    /// </summary>
    public string ChatModel { get; set; } = "gpt-3.5-turbo-0125";

    /// <summary>
    /// 端点
    /// </summary>
    public string Endpoint { get; set; } = "https://open666.cn";

    /// <summary>
    /// ApiKey
    /// </summary>
    public string ApiKey { get; set; } = "your-api key";
}