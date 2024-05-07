

import { Flexbox } from 'react-layout-kit';
import { Divider } from "antd"
import ChatListMessage from './feautres/ChatListMessage';
import { ActionIcon, ChatInputActionBar, MobileChatInputArea, MobileChatSendButton } from '@lobehub/ui';
import { Eraser, Languages } from 'lucide-react';

export default function Home() {
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
                <ChatListMessage messages={[
                    {
                        content: 'dayjs 如何使用 fromNow',
                        createAt: 1_686_437_950_084,
                        extra: {},
                        id: '1',
                        meta: {
                            avatar: '😄',
                            title: '我',
                        },
                        role: 'user',
                        updateAt: 1_686_437_950_084,
                    },
                    {
                        content:
                            '要使用 dayjs 的 fromNow 函数，需要先安装 dayjs 库并在代码中引入它。然后，可以使用以下语法来获取当前时间与给定时间之间的相对时间：\n\n```javascript\ndayjs().fromNow(); // 获取当前时间的相对时间\ndayjs(\'2021-05-01\').fromNow(); // 获取给定时间的相对时间\n```\n\n第一个示例将返回类似于 "几秒前"、"一分钟前"、"2 天前" 的相对时间字符串，表示当前时间与调用 fromNow 方法时的时间差。第二个示例将返回给定时间与当前时间的相对时间字符串。',
                        createAt: 1_686_538_950_084,
                        extra: {},
                        id: '2',
                        meta: {
                            avatar: '🤖',
                            backgroundColor: '#E8DA5A',
                            title: '智能助手',
                        },
                        role: 'assistant',
                        updateAt: 1_686_538_950_084,
                    },]} onDelete={function (id: string): void {
                        throw new Error('Function not implemented.');
                    }} onEdit={function (id: string, content: string): void {
                        throw new Error('Function not implemented.');
                    }} onRefresh={function (id: string): void {
                        throw new Error('Function not implemented.');
                    }} />
            </Flexbox>
            <MobileChatInputArea
                textAreaRightAddons={<MobileChatSendButton />}
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
        </Flexbox>
    )
}