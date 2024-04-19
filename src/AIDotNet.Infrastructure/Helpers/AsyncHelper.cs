namespace AIDotNet.Infrastructure.Helpers;

public static class AsyncHelper
{
    public static TResult RunSync<TResult>(Func<Task<TResult>> func)
    {
        return Task.Run(async () => await func().ConfigureAwait(false)).Result;
    }

    public static void RunSync(Func<Task> func)
    {
        Task.Run(async () => await func().ConfigureAwait(false)).Wait();
    }
}
