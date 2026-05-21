using System.Text.Json.Serialization;

namespace MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Models;

public sealed class IbgeImmediateRegionResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("nome")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("regiao-intermediaria")]
    public IbgeIntermediateRegionResponse IntermediateRegion { get; init; } = null!;
}
