namespace MunicipiosIbge.Api.Infrastructure.Cache.Options;

public sealed class CacheOptions
{
    public const string SectionName = "Cache";

    public int DefaultExpirationMinutes { get; init; } = 30;
}
