using Microsoft.AspNetCore.Mvc;
using MunicipiosIbge.Api.Common.Mediator;

namespace MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;

public sealed class GetMunicipalitiesQuery : IRequest<GetMunicipalitiesResponse>
{
    /// <summary>Busca parcial pelo nome do município.</summary>
    public string? Name { get; init; }

    /// <summary>Código IBGE da grande região.</summary>
    public int? RegionId { get; init; }

    /// <summary>Sigla da grande região. Exemplos: N, NE, CO, SE, S.</summary>
    public string? RegionAcronym { get; init; }

    /// <summary>Código IBGE da unidade federativa.</summary>
    public int? StateId { get; init; }

    /// <summary>Sigla da unidade federativa. Exemplos: MT, SP, RJ.</summary>
    [FromQuery(Name = "UF")]
    public string? Uf { get; init; }

    /// <summary>Alias legado para UF.</summary>
    public string? StateAcronym { get; init; }

    /// <summary>Código IBGE da mesorregião.</summary>
    public int? MesorregionId { get; init; }

    /// <summary>Código IBGE da microrregião.</summary>
    public int? MicroregionId { get; init; }

    /// <summary>Código IBGE da região intermediária.</summary>
    public int? IntermediateRegionId { get; init; }

    /// <summary>Código IBGE da região imediata.</summary>
    public int? ImmediateRegionId { get; init; }

    /// <summary>Página desejada. Se omitida, todos os itens são retornados.</summary>
    public int? Page { get; init; }

    /// <summary>Tamanho da página. Valor permitido: 1 a 200.</summary>
    public int? PageSize { get; init; }
}
