using MunicipiosIbge.Api.Common.Mediator;
using MunicipiosIbge.Api.Common.Responses;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Docs;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Models;

namespace MunicipiosIbge.Api.Features.Municipalities.GetById.Endpoints;

public static class GetMunicipalityByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetMunicipalityByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/municipalities/{id:int}", async (
                int id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.SendAsync(new GetMunicipalityByIdQuery(id), cancellationToken);

                return Results.Ok(ApiResponse<GetMunicipalityByIdResponse>.Ok(response));
            })
            .WithName("GetMunicipalityById")
            .WithTags("Municipalities")
            .WithSummary(GetMunicipalityByIdEndpointDocs.Summary)
            .WithDescription(GetMunicipalityByIdEndpointDocs.Description)
            .Produces<ApiResponse<GetMunicipalityByIdResponse>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .Produces<ErrorResponse>(StatusCodes.Status502BadGateway)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
