using AIDotNet.Document.Contract.Models;

namespace AIDotNet.Document.Contract.Services;

public interface IChatService
{
    /// <summary>
    /// 获取聊天消息
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<PageResult<ChatMessage>> GetChatMessagesAsync(int page, int pageSize);
    
    /// <summary>
    /// 删除聊天消息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RemoveChatMessageAsync(string id);
    
    /// <summary>
    /// 添加聊天消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task AddChatMessageAsync(ChatMessage message);
    
    /// <summary>
    /// 编辑聊天消息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    Task UpdateChatMessageAsync(string id,string content);
    
    /// <summary>
    /// 清空所有聊天消息
    /// </summary>
    /// <returns></returns>
    Task RemoveChatMessagesAsync();
}