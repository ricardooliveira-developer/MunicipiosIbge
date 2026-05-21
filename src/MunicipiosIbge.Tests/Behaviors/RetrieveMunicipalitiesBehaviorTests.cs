using Microsoft.Extensions.Logging.Abstractions;
using MunicipiosIbge.Api.Common.Behaviors;
using MunicipiosIbge.Api.Domain.Entities;
using MunicipiosIbge.Api.Infrastructure.Cache.Models;
using MunicipiosIbge.Tests.Common.Builders;
using MunicipiosIbge.Tests.Common.Fakes;

namespace MunicipiosIbge.Tests.Behaviors;

public sealed class RetrieveMunicipalitiesBehaviorTests
{
    [Fact]
    public async Task RetrieveAsync_WhenCacheHasValidMunicipalities_ReturnsCacheWithoutDatabaseOrSync()
    {
        var municipalities = new List<Municipality>
        {
            MunicipalityBuilder.Create(id: 1)
        };
        var cache = new FakeMunicipalityCacheService();
        await cache.SetAsync(MunicipalityCacheKey.All(), municipalities);
        var repository = new FakeMunicipalityRepository();
        var syncService = new FakeMunicipalitySyncService();
        var behavior = CreateBehavior(cache, repository, syncService);

        var result = await behavior.RetrieveAsync();

        Assert.Single(result);
        Assert.Equal(0, repository.GetAllCalls);
        Assert.Equal(0, syncService.SyncCalls);
    }

    [Fact]
    public async Task RetrieveAsync_WhenCacheIsEmptyAndDatabaseHasData_StoresDatabaseDataInCache()
    {
        var municipalities = new List<Municipality>
        {
            MunicipalityBuilder.Create(id: 1)
        };
        var cache = new FakeMunicipalityCacheService();
        var repository = new FakeMunicipalityRepository(municipalities);
        var syncService = new FakeMunicipalitySyncService();
        var behavior = CreateBehavior(cache, repository, syncService);

        var result = await behavior.RetrieveAsync();

        Assert.Single(result);
        Assert.Equal(1, repository.GetAllCalls);
        Assert.Equal(0, syncService.SyncCalls);
        Assert.Equal(1, cache.SetCalls);
    }

    [Fact]
    public async Task RetrieveAsync_WhenCacheAndDatabaseAreEmpty_RunsSync()
    {
        var cache = new FakeMunicipalityCacheService();
        var repository = new FakeMunicipalityRepository();
        var syncService = new FakeMunicipalitySyncService();
        var behavior = CreateBehavior(cache, repository, syncService);

        await behavior.RetrieveAsync();

        Assert.Equal(1, syncService.SyncCalls);
        Assert.True(repository.GetAllCalls >= 2);
    }

    private static RetrieveMunicipalitiesBehavior CreateBehavior(
        FakeMunicipalityCacheService cache,
        FakeMunicipalityRepository repository,
        FakeMunicipalitySyncService syncService)
    {
        return new RetrieveMunicipalitiesBehavior(
            cache,
            repository,
            syncService,
            NullLogger<RetrieveMunicipalitiesBehavior>.Instance);
    }
}
