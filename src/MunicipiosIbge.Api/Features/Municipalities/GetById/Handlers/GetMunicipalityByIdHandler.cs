using MunicipiosIbge.Api.Common.Behaviors;
using MunicipiosIbge.Api.Common.Exceptions;
using MunicipiosIbge.Api.Common.Mediator;
using MunicipiosIbge.Api.Features.Municipalities.Common;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Models;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Validators;

namespace MunicipiosIbge.Api.Features.Municipalities.GetById.Handlers;

public sealed class GetMunicipalityByIdHandler(
    IRetrieveMunicipalitiesBehavior retrieveMunicipalitiesBehavior,
    ILogger<GetMunicipalityByIdHandler> logger)
    : IRequestHandler<GetMunicipalityByIdQuery, GetMunicipalityByIdResponse>
{
    public async Task<GetMunicipalityByIdResponse> HandleAsync(
        GetMunicipalityByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        GetMunicipalityByIdValidator.Validate(request);

        logger.LogInformation("Consulta por ID iniciada: buscando município {MunicipalityId}.", request.Id);

        var municipalities = await retrieveMunicipalitiesBehavior.RetrieveAsync(cancellationToken);
        var municipality = municipalities.FirstOrDefault(municipality => municipality.Id == request.Id)
            ?? throw new NotFoundException($"Municipality '{request.Id}' was not found.");

        logger.LogInformation(
            "Consulta por ID concluída: município {MunicipalityId} encontrado ({MunicipalityName}).",
            municipality.Id,
            municipality.Name);

        return new GetMunicipalityByIdResponse(MunicipalityResponseMapper.ToResponse(municipality));
    }
}
