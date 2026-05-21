using System.Text.Json.Serialization;

namespace MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Models;

public sealed class IbgeRegionResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("sigla")]
    public string Acronym { get; init; } = string.Empty;

    [JsonPropertyName("nome")]
    public string Name { get; init; } = string.Empty;
}
