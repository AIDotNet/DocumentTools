declare var chatService: any;

/**
 * 获取聊天消息
 * @param page 
 * @param pageSize 
 * @returns 
 */
export function GetChatMessages(page: number, pageSize: number): PageResult<ChatMessage> {
    return chatService.GetChatMessages(page, pageSize);
}

/**
 * 删除聊天消息
 * @param id 
 * @returns 
 */
export function RemoveChatMessage(id: string) {
    return chatService.RemoveChatMessage(id);
}

/**
 * 添加聊天消息
 * @param data 
 * @returns 
 */
export function AddChatMessage(data: ChatMessage) {
    return chatService.AddChatMessage(data);
}

/**
 * 编辑聊天消息
 * @param id 编辑id
 * @param content 编辑内容
 * @returns 
 */
export function UpdateChatMessage(id: string, content: string) {
    return chatService.UpdateChatMessage(id, content);
}

/**
 * 清空所有聊天消息
 * @returns 
 */
export function RemoveChatMessages() {
    return chatService.RemoveChatMessages();
}

export interface ChatMessage {
    Content: string | null;
    CreateAt: string;
    Extra: { [key: string]: string; };
    Id: string | null;
    Meta: { [key: string]: string; };
    Role: string | null;
    UpdateAt: string | null;
}

export interface ChatMessageRole {
    Assistant: string;
    User: string;
    System: string;
    Tools: string;
}

export interface PageResult<T> {
    Total: number;
    Items: T[];
}