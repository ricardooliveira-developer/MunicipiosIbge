using MunicipiosIbge.Api.Features.Municipalities.GetAll.Endpoints;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Endpoints;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Endpoints;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Endpoints;

namespace MunicipiosIbge.Api.Common.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetMunicipalitiesEndpoint();
        app.MapGetMunicipalityByIdEndpoint();
        app.MapGetMunicipalitiesByMesorregionEndpoint();
        app.MapSyncMunicipalitiesEndpoint();

        return app;
    }
}
