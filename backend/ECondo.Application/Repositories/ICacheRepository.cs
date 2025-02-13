using Microsoft.Extensions.Caching.Distributed;

namespace ECondo.Application.Repositories;

public interface ICacheRepository
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
    Task StoreAsync<T>(string key, T value, DistributedCacheEntryOptions? entryOptions = null, CancellationToken cancellationToken = default) where T : class;
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
