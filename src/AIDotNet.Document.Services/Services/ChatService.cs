using AIDotNet.Document.Services.Domain;
using MapsterMapper;

namespace AIDotNet.Document.Services.Services;

public sealed class ChatService(IFreeSql freeSql, IMapper mapper) : IChatService
{
    public async Task<PageResult<ChatMessageDto>> GetChatMessagesAsync(int page, int pageSize)
    {
        var query = await freeSql.Select<ChatMessage>()
            .OrderByDescending(x => x.CreateAt)
            .Page(page, pageSize)
            .ToListAsync();

        var total = await freeSql.Select<ChatMessage>().CountAsync();

        return new PageResult<ChatMessageDto>(total, mapper.Map<List<ChatMessageDto>>(query));
    }

    public async Task RemoveChatMessageAsync(string id)
    {
        await freeSql.Delete<ChatMessage>().Where(x => x.Id == id).ExecuteAffrowsAsync();
    }

    public Task AddChatMessageAsync(ChatMessageDto message)
    {
        // message.Id = Guid.NewGuid().ToString("N");

        return freeSql.Insert(message).ExecuteAffrowsAsync();
    }

    public async Task UpdateChatMessageAsync(string id, string content)
    {
        await freeSql.Update<ChatMessage>()
            .Where(x => x.Id == id)
            .Set(x => x.Content, content)
            .ExecuteAffrowsAsync();
    }

    public Task RemoveChatMessagesAsync()
    {
        return freeSql.Delete<ChatMessage>().ExecuteAffrowsAsync();
    }
}