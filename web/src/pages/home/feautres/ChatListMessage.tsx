import {
    ActionEvent,
    ActionsBar,
    ActionsBarProps,
    ChatList,
    ChatListProps,
    ChatMessage,
    StoryBook,
    useControls,
    useCreateStore,
} from '@lobehub/ui';
import { JSX } from 'react/jsx-runtime';


interface ChatListMessageProps {
    messages: ChatMessage[];
    onDelete: (id: string) => void;
    onEdit: (id: string, content: string) => void;
    onRefresh: (id: string) => void;
}

export default function ChatListMessage({ messages, onDelete, onEdit, onRefresh }: ChatListMessageProps) {
    return (
        <ChatList
            data={messages}
            showTitle={true}
            type='chat'
            onActionsClick={(action: ActionEvent, message) => {

                if (action.key === 'delete') {
                    onDelete(message.id)
                } else if (action.key === 'edit') {
                    onEdit(message.id, message.content)
                } else if (action.key === 'regenerate') {
                    onRefresh(message.id)
                }
            }
            }
            renderActions={{
                "default": (props: ActionsBarProps) => <ActionsBar 
                    text={
                        {
                            copy: '复制',
                            delete: '删除',
                            edit: '编辑',
                            regenerate: '刷新'
                        }
                    }
                    {...props}
                />
            }}
            style={{
                padding: 8,
            }}
            renderMessages={{
                default: ({ id, editableContent }) => <div id={id}>{editableContent}</div>,
            }}
        />
    )
}