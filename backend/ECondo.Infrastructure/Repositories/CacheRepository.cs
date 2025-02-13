using ECondo.Application.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ECondo.Infrastructure.Repositories;

internal class CacheRepository(IDistributedCache distributedCache) : ICacheRepository
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        cancellationToken.ThrowIfCancellationRequested();

        string? cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);
        if (cachedValue is null)
            return null;

        T? value = JsonConvert.DeserializeObject<T>(cachedValue,
        new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        });

        return value;
    }

    public Task StoreAsync<T>(string key, T value, DistributedCacheEntryOptions? entryOptions = null,
        CancellationToken cancellationToken = default) where T : class
    {
        cancellationToken.ThrowIfCancellationRequested();
        string cachedValue = JsonConvert.SerializeObject(value);
        if (entryOptions is not null)
            return distributedCache.SetStringAsync(key, cachedValue, entryOptions, cancellationToken);

        return distributedCache.SetStringAsync(key, cachedValue, cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return distributedCache.RemoveAsync(key, cancellationToken);
    }
}
