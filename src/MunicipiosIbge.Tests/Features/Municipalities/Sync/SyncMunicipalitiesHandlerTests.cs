using MunicipiosIbge.Api.Features.Municipalities.Sync.Handlers;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Models;
using MunicipiosIbge.Tests.Common.Fakes;

namespace MunicipiosIbge.Tests.Features.Municipalities.Sync;

public sealed class SyncMunicipalitiesHandlerTests
{
    [Fact]
    public async Task HandleAsync_DelegatesSynchronizationToService()
    {
        var response = new SyncMunicipalitiesResponse(
            TotalReceived: 10,
            MunicipalitiesDeleted: 2,
            MunicipalitiesInserted: 10,
            CachedKeys: 12);
        var syncService = new FakeMunicipalitySyncService(response);
        var handler = new SyncMunicipalitiesHandler(syncService);

        var result = await handler.HandleAsync(new SyncMunicipalitiesCommand());

        Assert.Equal(1, syncService.SyncCalls);
        Assert.Equal(response, result);
    }
}
