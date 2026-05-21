using System.Text.Json.Serialization;

namespace MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Models;

public sealed class IbgeMicroregionResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("nome")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("mesorregiao")]
    public IbgeMesorregionResponse Mesorregion { get; init; } = null!;
}
