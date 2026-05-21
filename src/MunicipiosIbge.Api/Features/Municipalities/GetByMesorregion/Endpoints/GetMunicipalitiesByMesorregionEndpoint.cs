using Microsoft.AspNetCore.Mvc;
using MunicipiosIbge.Api.Common.Mediator;
using MunicipiosIbge.Api.Common.Responses;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Docs;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Models;

namespace MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Endpoints;

public static class GetMunicipalitiesByMesorregionEndpoint
{
    public static IEndpointRouteBuilder MapGetMunicipalitiesByMesorregionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/municipalities/mesorregions/{mesorregionId:int}", async (
                int mesorregionId,
                [FromQuery] int? page,
                [FromQuery] int? pageSize,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.SendAsync(
                    new GetMunicipalitiesByMesorregionQuery
                    {
                        MesorregionId = mesorregionId,
                        Page = page,
                        PageSize = pageSize
                    },
                    cancellationToken);

                return Results.Ok(ApiResponse<GetMunicipalitiesByMesorregionResponse>.Ok(response));
            })
            .WithName("GetMunicipalitiesByMesorregion")
            .WithTags("Municipalities")
            .WithSummary(GetMunicipalitiesByMesorregionEndpointDocs.Summary)
            .WithDescription(GetMunicipalitiesByMesorregionEndpointDocs.Description)
            .Produces<ApiResponse<GetMunicipalitiesByMesorregionResponse>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .Produces<ErrorResponse>(StatusCodes.Status502BadGateway)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
