import { callService } from "./framework";

/**
 * C#的AIDotNet.Document.Services.Services的ChatService服务
 */
const serviceName = 'AIDotNet.Document.Contract.Services.IChatService,AIDotNet.Document.Contract';

/**
 * 获取聊天消息
 * @param page 
 * @param pageSize 
 * @returns 
 */
export async function GetChatMessages(page: number, pageSize: number): Promise<PageResult<ChatMessage>> {
    return await callService(serviceName, 'GetChatMessagesAsync', page, pageSize );
}

/**
 * 删除聊天消息
 * @param id 
 * @returns 
 */
export async function RemoveChatMessage(id: string) {
    await callService(serviceName, 'RemoveChatMessageAsync', id);
}

/**
 * 添加聊天消息
 * @param data 
 * @returns 
 */
export async function AddChatMessage(data: ChatMessage) {
    await callService(serviceName, 'AddChatMessageAsync', data);
}

/**
 * 编辑聊天消息
 * @param id 编辑id
 * @param content 编辑内容
 * @returns 
 */
export async function UpdateChatMessage(id: string, content: string) {
    await callService(serviceName, 'UpdateChatMessageAsync', id, content);
}

/**
 * 清空所有聊天消息
 * @returns 
 */
export async function RemoveChatMessages() {
    await callService(serviceName, 'RemoveChatMessagesAsync');
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