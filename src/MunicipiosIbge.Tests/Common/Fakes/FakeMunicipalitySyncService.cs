using MunicipiosIbge.Api.Features.Municipalities.Sync.Models;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Services;

namespace MunicipiosIbge.Tests.Common.Fakes;

public sealed class FakeMunicipalitySyncService(SyncMunicipalitiesResponse? response = null) : IMunicipalitySyncService
{
    public int SyncCalls { get; private set; }

    public Task<SyncMunicipalitiesResponse> SyncAsync(CancellationToken cancellationToken = default)
    {
        SyncCalls++;

        return Task.FromResult(response ?? new SyncMunicipalitiesResponse(
            TotalReceived: 0,
            MunicipalitiesDeleted: 0,
            MunicipalitiesInserted: 0,
            CachedKeys: 0));
    }
}
