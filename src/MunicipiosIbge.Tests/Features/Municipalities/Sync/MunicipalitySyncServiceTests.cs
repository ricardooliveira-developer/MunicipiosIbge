using Microsoft.Extensions.Logging.Abstractions;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Services;
using MunicipiosIbge.Tests.Common.Builders;
using MunicipiosIbge.Tests.Common.Fakes;

namespace MunicipiosIbge.Tests.Features.Municipalities.Sync;

public sealed class MunicipalitySyncServiceTests
{
    [Fact]
    public async Task SyncAsync_WhenMunicipalityHasNoMicroregion_ReplacesDatabaseAndCachesMunicipality()
    {
        var ibgeApi = new FakeIbgeMunicipalityApi([
            IbgeMunicipalityBuilder.Create(id: 5101837, name: "Boa Esperança do Norte", withMicroregion: false)
        ]);
        var repository = new FakeMunicipalityRepository([
            MunicipalityBuilder.Create(id: 1)
        ]);
        var cache = new FakeMunicipalityCacheService();
        var service = CreateService(ibgeApi, repository, cache);

        var result = await service.SyncAsync();

        Assert.Equal(1, ibgeApi.Calls);
        Assert.Equal(1, repository.ReplaceAllCalls);
        Assert.Empty(repository.LastReplacedMicroregions);
        var municipality = Assert.Single(repository.LastReplacedMunicipalities);
        Assert.Equal(5101837, municipality.Id);
        Assert.Null(municipality.MicroregionId);
        Assert.Equal(1, result.MunicipalitiesDeleted);
        Assert.Equal(1, result.MunicipalitiesInserted);
        Assert.True(cache.SetCalls > 0);
    }

    [Fact]
    public async Task SyncAsync_WhenIbgeReturnsNoMunicipalities_ThrowsInvalidOperationException()
    {
        var service = CreateService(
            new FakeIbgeMunicipalityApi([]),
            new FakeMunicipalityRepository(),
            new FakeMunicipalityCacheService());

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.SyncAsync());
    }

    [Fact]
    public async Task SyncAsync_WhenImmediateRegionIsMissing_ThrowsInvalidOperationException()
    {
        var service = CreateService(
            new FakeIbgeMunicipalityApi([
                IbgeMunicipalityBuilder.Create(withImmediateRegion: false)
            ]),
            new FakeMunicipalityRepository(),
            new FakeMunicipalityCacheService());

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.SyncAsync());
    }

    private static MunicipalitySyncService CreateService(
        FakeIbgeMunicipalityApi ibgeApi,
        FakeMunicipalityRepository repository,
        FakeMunicipalityCacheService cache)
    {
        return new MunicipalitySyncService(
            ibgeApi,
            repository,
            cache,
            NullLogger<MunicipalitySyncService>.Instance);
    }
}
