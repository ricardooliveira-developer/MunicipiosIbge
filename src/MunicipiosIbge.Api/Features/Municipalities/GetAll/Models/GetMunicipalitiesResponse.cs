using System.Text.Json.Serialization;
using MunicipiosIbge.Api.Common.Responses;

namespace MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;

/// <summary>
/// Resultado paginado da consulta de municípios.
/// </summary>
/// <param name="Items">Municípios retornados na página atual.</param>
/// <param name="Page">Página retornada. Quando paginação não é informada, assume 1.</param>
/// <param name="PageSize">Quantidade de itens retornados por página. Quando paginação não é informada, equivale ao total encontrado.</param>
/// <param name="TotalItems">Quantidade total de municípios encontrados após aplicar os filtros.</param>
/// <param name="TotalPages">Quantidade total de páginas disponíveis.</param>
public sealed record GetMunicipalitiesResponse(
    IReadOnlyList<MunicipalityResponse> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages)
{
    public static GetMunicipalitiesResponse FromPagedResponse(PagedResponse<MunicipalityResponse> response)
    {
        return new GetMunicipalitiesResponse(
            response.Items,
            response.Page,
            response.PageSize,
            response.TotalItems,
            response.TotalPages);
    }
}

/// <summary>
/// Representação simplificada de um município do IBGE.
/// </summary>
/// <param name="Id">Código oficial do município no IBGE.</param>
/// <param name="Name">Nome oficial do município.</param>
/// <param name="Uf">Sigla da unidade federativa, por exemplo MT, SP ou RJ.</param>
/// <param name="StateId">Código oficial da unidade federativa no IBGE.</param>
/// <param name="StateName">Nome da unidade federativa.</param>
/// <param name="RegionId">Código oficial da grande região no IBGE.</param>
/// <param name="RegionAcronym">Sigla da grande região, por exemplo N, NE, CO, SE ou S.</param>
/// <param name="RegionName">Nome da grande região.</param>
/// <param name="MesorregionId">Código da mesorregião. Pode ser nulo quando o IBGE não retorna microrregião para o município.</param>
/// <param name="MesorregionName">Nome da mesorregião. Pode ser nulo quando não houver microrregião.</param>
/// <param name="MicroregionId">Código da microrregião. Pode ser nulo em municípios sem microrregião no payload do IBGE.</param>
/// <param name="MicroregionName">Nome da microrregião. Pode ser nulo em municípios sem microrregião.</param>
/// <param name="ImmediateRegionId">Código da região imediata.</param>
/// <param name="ImmediateRegionName">Nome da região imediata.</param>
/// <param name="IntermediateRegionId">Código da região intermediária.</param>
/// <param name="IntermediateRegionName">Nome da região intermediária.</param>
public sealed record MunicipalityResponse(
    int Id,
    string Name,
    [property: JsonPropertyName("UF")] string? Uf,
    int? StateId,
    string? StateName,
    int? RegionId,
    string? RegionAcronym,
    string? RegionName,
    int? MesorregionId,
    string? MesorregionName,
    int? MicroregionId,
    string? MicroregionName,
    int ImmediateRegionId,
    string ImmediateRegionName,
    int IntermediateRegionId,
    string IntermediateRegionName);
