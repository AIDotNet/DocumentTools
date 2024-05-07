

import { Flexbox } from 'react-layout-kit';
import { Divider, Button } from "antd"
import ChatListMessage from './feautres/ChatListMessage';
import { ActionIcon, ChatInputActionBar, MobileChatInputArea, MobileChatSendButton } from '@lobehub/ui';
import { Eraser, Languages } from 'lucide-react';
import { useEffect, useState } from 'react';
import {
    ChatMessage,
} from '@lobehub/ui';
import { AddChatMessage, GetChatMessages } from '../../services/chatService';
import React from 'react';

export default function Home() {
    const [messages, setMessages] = useState([] as ChatMessage[])
    const inputRef = React.createRef<any>()
    const [value, setValue] = useState('' as string)
    const [input, setInput] = useState({
        page: 1,
        pageSize: 10
    })

    function LoadMessages() {
        const message = GetChatMessages(input.page, input.pageSize);
        if (message?.Items) {
            setMessages(message.Items.map((item) => {
                // 将item.CreateAt转换1686538950084 这种时间戳格式
                const createAt = parseInt(item.CreateAt);
                const updateAt = item.UpdateAt ? parseInt(item.UpdateAt) : null;
                return {
                    content: item.Content,
                    id: item.Id,
                    role: item.Role,
                    createAt: createAt,
                    updateAt: updateAt,
                    meta: item.Meta,
                    extra: item.Extra,
                } as ChatMessage
            }))
        }
    }

    useEffect(() => {
        LoadMessages()
    }, [])

    function SendMessage() {
        AddChatMessage({
            Content: value,
            Role: 'user',
            CreateAt: new Date().getTime().toString(),
            UpdateAt: null,
            Meta: {
                avatar: '😁',
                title: '我',
            },
            Extra: {

            },
            Id: '',
        })

        setValue('')
        LoadMessages()
    }

    return (
        <Flexbox style={{
            height: '100vh',
            position: 'relative',
            overflowX: 'hidden',
            overflowY: 'auto',
            width: '100%'
        }}>
            <div style={{ height: 35 }}>
                <div style={{
                    fontSize: 20,
                    fontWeight: 600,
                    textAlign: 'left',
                    padding: 15
                }}>
                    本地笔记智能助手
                </div>
            </div>
            <Divider />

            <Flexbox style={{
                overflowX: 'hidden',
                overflowY: 'auto',
                flex: 1,
                padding: 8
            }}>
                <ChatListMessage messages={messages} onDelete={function (id: string): void {
                    throw new Error('Function not implemented.');
                }} onEdit={function (id: string, content: string): void {
                    throw new Error('Function not implemented.');
                }} onRefresh={function (id: string): void {
                    throw new Error('Function not implemented.');
                }} />
            </Flexbox>
            <div style={{
                display: 'flex',
                width: '100%',
            }}>
                <div style={{
                    flex: 1
                }}>
                    <MobileChatInputArea
                        value={value}
                        onInput={(value) => {
                            setValue(value)
                        }}
                        onSend={() => SendMessage()}
                        ref={inputRef}
                        topAddons={
                            <ChatInputActionBar
                                leftAddons={
                                    <>
                                        <ActionIcon icon={Languages} />
                                        <ActionIcon icon={Eraser} />
                                    </>
                                }
                            />
                        }
                    />
                </div>
                <div style={{
                    display: 'flex',
                    // 靠下
                    alignItems: 'flex-end',
                    marginBottom: 10,
                    backgroundColor: '#FAFAFA',
                }}>
                    <MobileChatSendButton

                        onSend={() => SendMessage()}
                    />
                </div>

            </div>

        </Flexbox>
    )
}