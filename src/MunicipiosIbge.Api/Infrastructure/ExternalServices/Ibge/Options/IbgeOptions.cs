namespace MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Options;

public sealed class IbgeOptions
{
    public const string SectionName = "Ibge";

    public string BaseUrl { get; init; } = string.Empty;
    public int TimeoutSeconds { get; init; } = 60;
}
