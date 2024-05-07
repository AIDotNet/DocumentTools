namespace AIDotNet.Document.Services.Services;

public sealed class ChatService(IFreeSql freeSql) : IChatService
{
    public async Task<PageResult<ChatMessage>> GetChatMessagesAsync(int page, int pageSize)
    {
        var query = await freeSql.Select<ChatMessage>()
            .OrderByDescending(x => x.CreateAt)
            .Page(page, pageSize)
            .ToListAsync();

        var total = await freeSql.Select<ChatMessage>().CountAsync();

        return new PageResult<ChatMessage>(total, query);
    }

    public async Task RemoveChatMessageAsync(string id)
    {
        await freeSql.Delete<ChatMessage>().Where(x => x.Id == id).ExecuteAffrowsAsync();
    }

    public Task AddChatMessageAsync(ChatMessage message)
    {
        message.Id = Guid.NewGuid().ToString("N");

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