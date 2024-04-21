using AIDotNet.Document.Contract.Models;

namespace AIDotNet.Document.Contract.Services;

public interface IKernelService
{
    /// <summary>
    /// 向量对话    
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    IAsyncEnumerable<string> CompletionAsync(CompletionInput input);
}