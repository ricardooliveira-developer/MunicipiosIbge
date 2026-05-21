using System.Text.Json.Serialization;

namespace MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Models;

public sealed class IbgeMesorregionResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("nome")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("uf")]
    public IbgeStateResponse State { get; init; } = null!;
}
