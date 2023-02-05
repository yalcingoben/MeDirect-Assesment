namespace ExchangeTrader.App.Abstractions.Caching
{
    public interface ICacheService
    {
        Task<string> GetValueAsync(string key, CancellationToken cancellationToken);
        Task<bool> SetValueAsync(string key, string value, TimeSpan expireTime, CancellationToken cancellationToken);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action, TimeSpan expireTime, CancellationToken cancellationToken) where T : class;
        T GetOrAdd<T>(string key, Func<T> action, TimeSpan expireTime, CancellationToken cancellationToken) where T : class;
        Task Clear(string key, CancellationToken cancellationToken);
        void ClearAll(CancellationToken cancellationToken);
    }
}
