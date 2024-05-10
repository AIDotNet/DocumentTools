declare var window: any;
/**
 * 发送服务
 */
const sendCallServiceProtocol = 'sendCallService:';

/**
 * 调用注入的服务
 */
const callServiceProtocol = 'callService:';
const responseEventHandlers: { [eventId: string]: { resolve: Function; reject: Function } } = {};

export function sendCallServiceMessage(serviceName: string, methodName: string, parameters: any): Promise<any> {
    return new Promise((resolve, reject) => {
        // 生成eventId 36位随机字符串
        let eventId = Uuid();
        // 将Promise的解析和拒绝函数存储在一个全局对象中
        responseEventHandlers[eventId] = { resolve, reject };

        // 发送消息
        window.chrome.webview.postMessage(sendCallServiceProtocol + JSON.stringify({
            ServiceName: serviceName,
            MethodName: methodName,
            Parameters: parameters,
            EventId: eventId
        }));
    });
}

export function callService(serviceName: string, methodName: string, ...parameters: any): Promise<any> {
    return new Promise((resolve, reject) => {
        // 生成eventId 36位随机字符串
        let eventId = Uuid();
        // 将Promise的解析和拒绝函数存储在一个全局对象中
        responseEventHandlers[eventId] = { resolve, reject };

        // 发送消息
        window.chrome.webview.postMessage(callServiceProtocol + JSON.stringify({
            ServiceName: serviceName,
            MethodName: methodName,
            Parameters: parameters,
            EventId: eventId
        }));
    })

}



export function sendCloseWindowMessage() {
    window.chrome.webview.postMessage('closeWindow');
}

export function sendMinimizeWindowMessage() {
    window.chrome.webview.postMessage('minimizeWindow');
}

export function Uuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0,
            v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

// 假设这是从后台接收消息的函数
function onMessageReceived(event: any) {
    try {
        // 假设后台返回的数据格式是 { EventId: string, Data: any }
        const { EventId, Result, Type } = JSON.parse(event.data);
        if (Type === "Task") {
            // 检查是否有对应的Promise处理函数
            const eventHandler = responseEventHandlers[EventId];
            if (eventHandler) {
                try {
                    // 解析Promise
                    eventHandler.resolve(Result);

                    // 清理处理器，避免内存泄漏
                    delete responseEventHandlers[EventId];
                } catch (e) {
                    // 处理解析错误
                    eventHandler.reject(e);
                    delete responseEventHandlers[EventId];
                }
            }
        } 
    } catch (error) {
        // 处理解析错误或者其他可能的错误
        console.error('Error handling message:', error);
    }
}

window.chrome.webview.addEventListener('message', onMessageReceived);