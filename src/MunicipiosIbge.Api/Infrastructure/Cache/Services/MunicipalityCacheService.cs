using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MunicipiosIbge.Api.Infrastructure.Cache.Interfaces;
using MunicipiosIbge.Api.Infrastructure.Cache.Options;

namespace MunicipiosIbge.Api.Infrastructure.Cache.Services;

public sealed class MunicipalityCacheService(
    IDistributedCache distributedCache,
    IOptions<CacheOptions> cacheOptions) : IMunicipalityCacheService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrWhiteSpace(cachedValue))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(cachedValue, SerializerOptions);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        CancellationToken cancellationToken = default)
    {
        var serializedValue = JsonSerializer.Serialize(value, SerializerOptions);
        var expiration = absoluteExpirationRelativeToNow
            ?? TimeSpan.FromMinutes(cacheOptions.Value.DefaultExpirationMinutes);

        await distributedCache.SetStringAsync(
            key,
            serializedValue,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            },
            cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        return distributedCache.RemoveAsync(key, cancellationToken);
    }
}
