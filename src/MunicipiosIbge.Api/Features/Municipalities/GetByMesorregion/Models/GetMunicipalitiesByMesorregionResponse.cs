using MunicipiosIbge.Api.Common.Responses;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;

namespace MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Models;

/// <summary>
/// Resultado paginado da consulta de municípios por mesorregião.
/// </summary>
/// <param name="MesorregionId">Código da mesorregião usada como filtro.</param>
/// <param name="Items">Municípios encontrados para a mesorregião.</param>
/// <param name="Page">Página retornada.</param>
/// <param name="PageSize">Quantidade de itens retornados por página.</param>
/// <param name="TotalItems">Quantidade total de municípios encontrados para a mesorregião.</param>
/// <param name="TotalPages">Quantidade total de páginas disponíveis.</param>
public sealed record GetMunicipalitiesByMesorregionResponse(
    int MesorregionId,
    IReadOnlyList<MunicipalityResponse> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages)
{
    public static GetMunicipalitiesByMesorregionResponse FromPagedResponse(
        int mesorregionId,
        PagedResponse<MunicipalityResponse> response)
    {
        return new GetMunicipalitiesByMesorregionResponse(
            mesorregionId,
            response.Items,
            response.Page,
            response.PageSize,
            response.TotalItems,
            response.TotalPages);
    }
}
