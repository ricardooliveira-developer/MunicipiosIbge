using MunicipiosIbge.Api.Common.Mediator;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Models;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Services;

namespace MunicipiosIbge.Api.Features.Municipalities.Sync.Handlers;

public sealed class SyncMunicipalitiesHandler(IMunicipalitySyncService municipalitySyncService)
    : IRequestHandler<SyncMunicipalitiesCommand, SyncMunicipalitiesResponse>
{
    public Task<SyncMunicipalitiesResponse> HandleAsync(
        SyncMunicipalitiesCommand request,
        CancellationToken cancellationToken = default)
    {
        return municipalitySyncService.SyncAsync(cancellationToken);
    }
}
