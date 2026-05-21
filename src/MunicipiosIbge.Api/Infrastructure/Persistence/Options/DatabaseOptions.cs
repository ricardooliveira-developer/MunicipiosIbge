namespace MunicipiosIbge.Api.Infrastructure.Persistence.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    public bool ApplyMigrationsOnStartup { get; init; } = true;
}
