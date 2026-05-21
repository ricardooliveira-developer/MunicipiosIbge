namespace MunicipiosIbge.Api.Infrastructure.Logging;

public static class LoggingExtensions
{
    public static void AddApplicationLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
        builder.Logging.AddProvider(new ReadableConsoleLoggerProvider());
        builder.Logging.AddDebug();
    }
}
