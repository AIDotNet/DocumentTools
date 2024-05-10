
/**
 * C#的AIDotNet.Document.Services.Services的ChatService服务
 */
const serviceName = 'AIDotNet.Document.Contract.Services.IKernelService,AIDotNet.Document.Contract';

// 一个IAsyncEnumerable的实现
export async function Completion(value: any): Promise<any> {
    return await callServiceIAsyncEnumerable(serviceName, 'CompletionAsync', value);
}