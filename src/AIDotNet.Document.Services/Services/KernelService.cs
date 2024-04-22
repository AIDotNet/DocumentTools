using AIDotNet.Document.Contract;
using OpenAI_API;
using OpenAI_API.Chat;
using ChatMessage = AIDotNet.Document.Contract.Models.ChatMessage;
using ChatMessageRole = AIDotNet.Document.Contract.Models.ChatMessageRole;

namespace AIDotNet.Document.Services.Services;

public class KernelService(IKernelMemory kernelMemory, ISettingService settingService)
    : IKernelService
{
    private const string PromptTemplate = """"
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


        // 在这里会将搜索结果的分区拼接起来
        foreach (var item in result.Results)
        {
            prompt += string.Join(Environment.NewLine, item.Partitions.Select(x => x.Text));
        }

        if (!string.IsNullOrEmpty(prompt))
        {
            // 替换模板的参数
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
        }


        var chatHistory = new List<OpenAI_API.Chat.ChatMessage>();

        var history = input.History.Select(x => new OpenAI_API.Chat.ChatMessage()
        {
            Content = x.Content,
            Role = OpenAI_API.Chat.ChatMessageRole.FromString(x.Role!.ToString()),
        }).ToList();

        chatHistory.AddRange(history);


        var options = settingService.GetSetting<OpenAIOptions>(Constant.Settings.OpenAIOptions);

        var api = new OpenAIAPI(options.ApiKey)
        {
            ApiUrlFormat = $"{options.Endpoint.TrimEnd('/')}/{{0}}/{{1}}",
        };

        var request = new ChatRequest()
        {
            Model = options.ChatModel,
            MaxTokens = 2048,
            Messages = chatHistory,
            Temperature = 0.5f,
            FrequencyPenalty = 0.5f,
            PresencePenalty = 0.5f,
        };

        await foreach (var item in api.Chat.StreamChatEnumerableAsync(request))
        {
            yield return item?.Choices?.FirstOrDefault()?.Message?.Content ?? string.Empty;
        }
    }
}