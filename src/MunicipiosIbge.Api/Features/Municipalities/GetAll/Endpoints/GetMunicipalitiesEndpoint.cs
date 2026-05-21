using Microsoft.AspNetCore.Mvc;
using MunicipiosIbge.Api.Common.Mediator;
using MunicipiosIbge.Api.Common.Responses;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Docs;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;

namespace MunicipiosIbge.Api.Features.Municipalities.GetAll.Endpoints;

public static class GetMunicipalitiesEndpoint
{
    public static IEndpointRouteBuilder MapGetMunicipalitiesEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/municipalities", async (
                [AsParameters] GetMunicipalitiesQuery query,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.SendAsync(query, cancellationToken);

                return Results.Ok(ApiResponse<GetMunicipalitiesResponse>.Ok(response));
            })
            .WithName("GetMunicipalities")
            .WithTags("Municipalities")
            .WithSummary(GetMunicipalitiesEndpointDocs.Summary)
            .WithDescription(GetMunicipalitiesEndpointDocs.Description)
            .Produces<ApiResponse<GetMunicipalitiesResponse>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status502BadGateway)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
