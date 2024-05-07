using System.Runtime.InteropServices;
using AIDotNet.Document.Contract.Models;

namespace AIDotNet.Document.Contract.Services;

[ComVisible(true)]
public interface IChatService
{
    /// <summary>
    /// 获取聊天消息
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    PageResult<ChatMessageDto> GetChatMessages(int page, int pageSize);

    /// <summary>
    /// 删除聊天消息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    void RemoveChatMessage(string id);
    
    /// <summary>
    /// 添加聊天消息
    /// </summary>
    /// <param name="messageDto"></param>
    /// <returns></returns>
    void AddChatMessage(ChatMessageDto messageDto);
    
    /// <summary>
    /// 编辑聊天消息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    void UpdateChatMessage(string id,string content);

    /// <summary>
    /// 清空所有聊天消息
    /// </summary>
    void RemoveChatMessages();
}