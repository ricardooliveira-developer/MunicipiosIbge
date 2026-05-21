using MunicipiosIbge.Api.Common.Behaviors;
using MunicipiosIbge.Api.Common.Mediator;
using MunicipiosIbge.Api.Common.Responses;
using MunicipiosIbge.Api.Domain.Entities;
using MunicipiosIbge.Api.Features.Municipalities.Common;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Validators;

namespace MunicipiosIbge.Api.Features.Municipalities.GetAll.Handlers;

public sealed class GetMunicipalitiesHandler(
    IRetrieveMunicipalitiesBehavior retrieveMunicipalitiesBehavior,
    ILogger<GetMunicipalitiesHandler> logger)
    : IRequestHandler<GetMunicipalitiesQuery, GetMunicipalitiesResponse>
{
    public async Task<GetMunicipalitiesResponse> HandleAsync(
        GetMunicipalitiesQuery request,
        CancellationToken cancellationToken = default)
    {
        GetMunicipalitiesValidator.Validate(request);

        logger.LogInformation(
            "Consulta de municípios iniciada. Filtros: nome={Name}, região={RegionId}/{RegionAcronym}, UF={StateId}/{Uf}, mesorregião={MesorregionId}, microrregião={MicroregionId}, região intermediária={IntermediateRegionId}, região imediata={ImmediateRegionId}, página={Page}, tamanho={PageSize}.",
            request.Name,
            request.RegionId,
            request.RegionAcronym,
            request.StateId,
            request.Uf ?? request.StateAcronym,
            request.MesorregionId,
            request.MicroregionId,
            request.IntermediateRegionId,
            request.ImmediateRegionId,
            request.Page,
            request.PageSize);

        var municipalities = await retrieveMunicipalitiesBehavior.RetrieveAsync(cancellationToken);
        var filteredMunicipalities = ApplyFilters(municipalities, request).ToList();
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
            "Consulta de municípios concluída: {TotalItems} registros encontrados e {ReturnedItems} retornados na página {Page} com tamanho {PageSize}.",
            totalItems,
            items.Count,
            page,
            pageSize);

        return GetMunicipalitiesResponse.FromPagedResponse(pagedResponse);
    }

    private static IEnumerable<Municipality> ApplyFilters(
        IEnumerable<Municipality> municipalities,
        GetMunicipalitiesQuery query)
    {
        var filteredMunicipalities = municipalities;

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            filteredMunicipalities = filteredMunicipalities.Where(municipality =>
                municipality.Name.Contains(query.Name, StringComparison.OrdinalIgnoreCase));
        }

        if (query.RegionId.HasValue)
        {
            filteredMunicipalities = filteredMunicipalities.Where(municipality =>
                municipality.ImmediateRegion?.IntermediateRegion?.State?.RegionId == query.RegionId
                || municipality.Microregion?.Mesorregion?.State?.RegionId == query.RegionId);
        }

        if (!string.IsNullOrWhiteSpace(query.RegionAcronym))
        {
            filteredMunicipalities = filteredMunicipalities.Where(municipality =>
                string.Equals(
                    municipality.ImmediateRegion?.IntermediateRegion?.State?.Region?.Acronym,
                    query.RegionAcronym,
                    StringComparison.OrdinalIgnoreCase)
                || string.Equals(
                    municipality.Microregion?.Mesorregion?.State?.Region?.Acronym,
                    query.RegionAcronym,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (query.StateId.HasValue)
        {
            filteredMunicipalities = filteredMunicipalities.Where(municipality =>
                municipality.ImmediateRegion?.IntermediateRegion?.StateId == query.StateId
                || municipality.Microregion?.Mesorregion?.StateId == query.StateId);
        }

        var uf = query.Uf ?? query.StateAcronym;

        if (!string.IsNullOrWhiteSpace(uf))
        {
            filteredMunicipalities = filteredMunicipalities.Where(municipality =>
                string.Equals(
                    municipality.ImmediateRegion?.IntermediateRegion?.State?.Acronym,
                    uf,
                    StringComparison.OrdinalIgnoreCase)
                || string.Equals(
                    municipality.Microregion?.Mesorregion?.State?.Acronym,
                    uf,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (query.MesorregionId.HasValue)
        {
            filteredMunicipalities = filteredMunicipalities.Where(municipality =>
                municipality.Microregion?.MesorregionId == query.MesorregionId);
        }

        if (query.MicroregionId.HasValue)
        {
            filteredMunicipalities = filteredMunicipalities.Where(municipality =>
                municipality.MicroregionId == query.MicroregionId);
        }

        if (query.IntermediateRegionId.HasValue)
        {
            filteredMunicipalities = filteredMunicipalities.Where(municipality =>
                municipality.ImmediateRegion?.IntermediateRegionId == query.IntermediateRegionId);
        }

        if (query.ImmediateRegionId.HasValue)
        {
            filteredMunicipalities = filteredMunicipalities.Where(municipality =>
                municipality.ImmediateRegionId == query.ImmediateRegionId);
        }

        return filteredMunicipalities.OrderBy(municipality => municipality.Name);
    }
}
