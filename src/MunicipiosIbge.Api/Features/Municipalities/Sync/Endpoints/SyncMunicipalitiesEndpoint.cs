using MunicipiosIbge.Api.Common.Mediator;
using MunicipiosIbge.Api.Common.Responses;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Docs;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Models;

namespace MunicipiosIbge.Api.Features.Municipalities.Sync.Endpoints;

public static class SyncMunicipalitiesEndpoint
{
    public static IEndpointRouteBuilder MapSyncMunicipalitiesEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/sync", async (IMediator mediator, CancellationToken cancellationToken) =>
            {
                var response = await mediator.SendAsync(new SyncMunicipalitiesCommand(), cancellationToken);

                return Results.Ok(ApiResponse<SyncMunicipalitiesResponse>.Ok(
                    response,
                    "Municipalities synchronized successfully."));
            })
            .WithName("SyncMunicipalities")
            .WithTags("Municipalities")
            .WithSummary(SyncMunicipalitiesEndpointDocs.Summary)
            .WithDescription(SyncMunicipalitiesEndpointDocs.Description)
            .Produces<ApiResponse<SyncMunicipalitiesResponse>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status502BadGateway)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
