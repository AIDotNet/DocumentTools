using System.Runtime.InteropServices;
using AIDotNet.Document.Services.Domain;

namespace AIDotNet.Document.Services.Services;

[ClassInterface(ClassInterfaceType.AutoDual)]
[ComVisible(true)]
public class ChatService(IFreeSql freeSql) : IChatService
{
    public PageResult<ChatMessageDto> GetChatMessages(int page, int pageSize)
    {
        var query = freeSql.Select<ChatMessage>()
            .OrderByDescending(x => x.CreateAt)
            .Page(page, pageSize)
            .ToList();

        var total = freeSql.Select<ChatMessage>().Count();

        return new PageResult<ChatMessageDto>(total, query.Select(x => new ChatMessageDto()
        {
            Content = x.Content,
            CreateAt = x.CreateAt,
            Extra = x.Extra,
            Id = x.Id,
            Meta = x.Meta,
            Role = x.Role,
            UpdateAt = x.UpdateAt,
        }).ToList());
    }

    public async void RemoveChatMessage(string id)
    {
        await freeSql.Delete<ChatMessage>().Where(x => x.Id == id).ExecuteAffrowsAsync();
    }

    public void AddChatMessage(ChatMessageDto messageDto)
    {
        messageDto.Id = Guid.NewGuid().ToString("N");

        freeSql.Insert(new ChatMessage()
        {
            Id = Guid.NewGuid().ToString("N"),
            Content = messageDto.Content,
            CreateAt = messageDto.CreateAt,
            Extra = messageDto.Extra,
            Meta = messageDto.Meta,
            Role = messageDto.Role,
            UpdateAt = messageDto.UpdateAt,
        }).ExecuteAffrows();
    }

    public async void UpdateChatMessage(string id, string content)
    {
        await freeSql.Update<ChatMessage>()
            .Where(x => x.Id == id)
            .Set(x => x.Content, content)
            .ExecuteAffrowsAsync();
    }

    public void RemoveChatMessages()
    {
        freeSql.Delete<ChatMessage>().ExecuteAffrows();
    }
}