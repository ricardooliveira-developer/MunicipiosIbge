using System.Text.Json.Serialization;

namespace MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Models;

public sealed class IbgeMunicipalityResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("nome")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("microrregiao")]
    public IbgeMicroregionResponse Microregion { get; init; } = null!;

    [JsonPropertyName("regiao-imediata")]
    public IbgeImmediateRegionResponse ImmediateRegion { get; init; } = null!;
}
