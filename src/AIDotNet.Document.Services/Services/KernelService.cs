using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AIDotNet.Document.Services.Services;

public class KernelService(IKernelMemory kernelMemory,IServiceProvider serviceProvider) : IKernelService
{
    private string PromptTemplate =
        """"
        使用 <data></data> 标记中的内容作为你的知识:
            <data>
            {{quote}}
            </data>

        回答要求：
        - 如果你不清楚答案，你需要澄清。
        - 避免提及你是从 <data></data> 获取的知识。
        - 保持答案与 <data></data> 中描述的一致。
        - 使用 Markdown 语法优化回答格式。
        - 使用与问题相同的语言回答。
        - 如果 Markdown中有图片则正常显示。

        问题:"""{{question}}"""
        """";

    public async IAsyncEnumerable<string> CompletionAsync(CompletionInput input)
    {
        // 向量搜索

        var content = input.History.Last();

        var result = await kernelMemory.SearchAsync(content.Content, "document", limit: 3,
            minRelevance: input.Relevancy);

        var prompt = string.Empty;


        foreach (var item in result.Results)
        {
            prompt += string.Join(Environment.NewLine, item.Partitions.Select(x => x.Text));
        }


        prompt = PromptTemplate.Replace("{{quote}}", prompt)
            .Replace("{{question}}", content.Content);

        // 往input.History最上面添加
        input.History.Insert(0, new ChatMessage()
        {
            Content = prompt,
            CreateAt = DateTime.Now,
            Extra = new Dictionary<string, string>(),
            Id = Guid.NewGuid().ToString(),
            Meta = new Dictionary<string, string>(),
            Role = ChatMessageRole.System
        });

        var kernel = serviceProvider.GetRequiredService<Kernel>();
        
        var chat = kernel.GetRequiredService<IChatCompletionService>();

        var chatHistory = new ChatHistory();

        var history = input.History.Select(x => new ChatMessageContent()
        {
            Content = x.Content,
            Role = new AuthorRole(x.Role),
        }).ToList();

        chatHistory.AddRange(history);

        await foreach (var item in chat.GetStreamingChatMessageContentsAsync(chatHistory))
        {
            yield return item.Content ?? string.Empty;
        }
    }
}