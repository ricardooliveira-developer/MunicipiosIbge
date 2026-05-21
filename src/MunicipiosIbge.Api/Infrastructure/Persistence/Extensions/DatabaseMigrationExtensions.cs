using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MunicipiosIbge.Api.Infrastructure.Persistence.Context;
using MunicipiosIbge.Api.Infrastructure.Persistence.Options;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Extensions;

public static class DatabaseMigrationExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

        if (!options.ApplyMigrationsOnStartup)
        {
            return;
        }

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<MunicipalitiesDbContext>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<MunicipalitiesDbContext>();

        logger.LogInformation("Verificando migrations do banco de dados.");

        await dbContext.Database.MigrateAsync();

        logger.LogInformation("Banco de dados pronto: migrations aplicadas com sucesso.");
    }
}
