using Microsoft.Extensions.Caching.Distributed;

namespace MunicipiosIbge.Tests.Common.Fakes;

public sealed class FakeDistributedCache : IDistributedCache
{
    private readonly Dictionary<string, byte[]> values = [];

    public int RemoveCalls { get; private set; }
    public DistributedCacheEntryOptions? LastOptions { get; private set; }

    public byte[]? Get(string key)
    {
        return values.GetValueOrDefault(key);
    }

    public Task<byte[]?> GetAsync(string key, CancellationToken token = default)
    {
        return Task.FromResult(Get(key));
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        values[key] = value;
        LastOptions = options;
    }

    public Task SetAsync(
        string key,
        byte[] value,
        DistributedCacheEntryOptions options,
        CancellationToken token = default)
    {
        Set(key, value, options);
        return Task.CompletedTask;
    }

    public void Refresh(string key)
    {
    }

    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        return Task.CompletedTask;
    }

    public void Remove(string key)
    {
        RemoveCalls++;
        values.Remove(key);
    }

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        Remove(key);
        return Task.CompletedTask;
    }
}
