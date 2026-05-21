using MunicipiosIbge.Api.Common.Behaviors;
using MunicipiosIbge.Api.Common.Exceptions;
using MunicipiosIbge.Api.Common.Mediator;
using MunicipiosIbge.Api.Common.Responses;
using MunicipiosIbge.Api.Features.Municipalities.Common;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Models;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Validators;

namespace MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Handlers;

public sealed class GetMunicipalitiesByMesorregionHandler(
    IRetrieveMunicipalitiesBehavior retrieveMunicipalitiesBehavior,
    ILogger<GetMunicipalitiesByMesorregionHandler> logger)
    : IRequestHandler<GetMunicipalitiesByMesorregionQuery, GetMunicipalitiesByMesorregionResponse>
{
    public async Task<GetMunicipalitiesByMesorregionResponse> HandleAsync(
        GetMunicipalitiesByMesorregionQuery request,
        CancellationToken cancellationToken = default)
    {
        GetMunicipalitiesByMesorregionValidator.Validate(request);

        logger.LogInformation(
            "Consulta por mesorregião iniciada: mesorregião={MesorregionId}, página={Page}, tamanho={PageSize}.",
            request.MesorregionId,
            request.Page,
            request.PageSize);

        var municipalities = await retrieveMunicipalitiesBehavior.RetrieveAsync(cancellationToken);
        var filteredMunicipalities = municipalities
            .Where(municipality => municipality.Microregion?.MesorregionId == request.MesorregionId)
            .OrderBy(municipality => municipality.Name)
            .ToList();

        if (filteredMunicipalities.Count == 0)
        {
            throw new NotFoundException($"No municipalities were found for mesorregion '{request.MesorregionId}'.");
        }

        var totalItems = filteredMunicipalities.Count;
        var page = request.Page ?? 1;
        var pageSize = request.PageSize ?? Math.Max(totalItems, 1);

        var items = filteredMunicipalities
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(MunicipalityResponseMapper.ToResponse)
            .ToList();

        var pagedResponse = PagedResponse<MunicipalityResponse>.Create(
            items,
            page,
            pageSize,
            totalItems);

        logger.LogInformation(
            "Consulta por mesorregião concluída: mesorregião={MesorregionId}, {TotalItems} registros encontrados e {ReturnedItems} retornados na página {Page} com tamanho {PageSize}.",
            request.MesorregionId,
            totalItems,
            items.Count,
            page,
            pageSize);

        return GetMunicipalitiesByMesorregionResponse.FromPagedResponse(
            request.MesorregionId,
            pagedResponse);
    }
}
