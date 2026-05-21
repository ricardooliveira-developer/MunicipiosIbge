using MunicipiosIbge.Api.Features.Municipalities.Sync.Models;

namespace MunicipiosIbge.Api.Features.Municipalities.Sync.Services;

public interface IMunicipalitySyncService
{
    Task<SyncMunicipalitiesResponse> SyncAsync(CancellationToken cancellationToken = default);
}
