using MunicipiosIbge.Api.Infrastructure.Cache.Interfaces;

namespace MunicipiosIbge.Tests.Common.Fakes;

public sealed class FakeMunicipalityCacheService : IMunicipalityCacheService
{
    private readonly Dictionary<string, object?> values = [];

    public int GetCalls { get; private set; }
    public int SetCalls { get; private set; }
    public int RemoveCalls { get; private set; }

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        GetCalls++;
        return Task.FromResult(values.TryGetValue(key, out var value) ? (T?)value : default);
    }

    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpirationRelativeToNow = null,
        CancellationToken cancellationToken = default)
    {
        SetCalls++;
        values[key] = value;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        RemoveCalls++;
        values.Remove(key);
        return Task.CompletedTask;
    }
}
