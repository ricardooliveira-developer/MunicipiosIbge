using Microsoft.Extensions.Options;
using MunicipiosIbge.Api.Infrastructure.Cache.Options;
using MunicipiosIbge.Api.Infrastructure.Cache.Services;
using MunicipiosIbge.Tests.Common.Fakes;

namespace MunicipiosIbge.Tests.Infrastructure.Cache;

public sealed class MunicipalityCacheServiceTests
{
    [Fact]
    public async Task GetAsync_WhenKeyDoesNotExist_ReturnsDefault()
    {
        var distributedCache = new FakeDistributedCache();
        var service = CreateService(distributedCache);

        var result = await service.GetAsync<TestValue>("missing");

        Assert.Null(result);
    }

    [Fact]
    public async Task SetAsync_StoresSerializedValueWithConfiguredExpiration()
    {
        var distributedCache = new FakeDistributedCache();
        var service = CreateService(distributedCache);

        await service.SetAsync("key", new TestValue("Sorriso"));

        var result = await service.GetAsync<TestValue>("key");

        Assert.NotNull(result);
        Assert.Equal("Sorriso", result.Name);
        Assert.Equal(TimeSpan.FromMinutes(15), distributedCache.LastOptions?.AbsoluteExpirationRelativeToNow);
    }

    [Fact]
    public async Task RemoveAsync_RemovesValueFromCache()
    {
        var distributedCache = new FakeDistributedCache();
        var service = CreateService(distributedCache);
        await service.SetAsync("key", new TestValue("Sorriso"));

        await service.RemoveAsync("key");

        Assert.Null(await service.GetAsync<TestValue>("key"));
        Assert.Equal(1, distributedCache.RemoveCalls);
    }

    private static MunicipalityCacheService CreateService(FakeDistributedCache distributedCache)
    {
        return new MunicipalityCacheService(
            distributedCache,
            Options.Create(new CacheOptions { DefaultExpirationMinutes = 15 }));
    }

    private sealed record TestValue(string Name);
}
