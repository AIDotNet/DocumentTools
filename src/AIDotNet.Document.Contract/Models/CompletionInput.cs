namespace AIDotNet.Document.Contract.Models;

public class CompletionInput
{
    /// <summary>
    /// 对话
    /// </summary>
    public List<ChatMessageDto> History { get; set; }

    /// <summary>
    /// 向量相似度
    /// </summary>
    public float Relevancy { get; set; }
}