namespace AIDotNet.Document.Contract.Services;

public enum VectorStatus
{
    /// <summary>
    /// 未处理
    /// </summary>
    Unhandled = 1,

    /// <summary>
    /// 处理中
    /// </summary>
    Processing = 2,

    /// <summary>
    /// 处理完成
    /// </summary>
    Processed = 3,

    /// <summary>
    /// 处理失败
    /// </summary>
    Failed = 4,
}