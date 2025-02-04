namespace ECommercePlatform.Shared.Utils.Async;

public sealed class AsyncLazy<T>
{
    private readonly Lazy<Task<T>> _lazyTask;

    public AsyncLazy(Func<T> valueFactory)
    {
        _lazyTask = new Lazy<Task<T>>(() => Task.Run(valueFactory));
    }

    public AsyncLazy(Func<Task<T>> taskFactory)
    {
        _lazyTask = new Lazy<Task<T>>(taskFactory);
    }
    
    public bool IsValueCreated => _lazyTask.IsValueCreated;

    public Task<T> Value => _lazyTask.Value;
}
