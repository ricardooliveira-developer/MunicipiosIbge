using Microsoft.EntityFrameworkCore;
using MunicipiosIbge.Api.Common.Behaviors;
using MunicipiosIbge.Api.Common.Mediator;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Handlers;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Handlers;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Models;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Handlers;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Models;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Handlers;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Models;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Services;
using MunicipiosIbge.Api.Infrastructure.Cache.Interfaces;
using MunicipiosIbge.Api.Infrastructure.Cache.Options;
using MunicipiosIbge.Api.Infrastructure.Cache.Services;
using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Clients;
using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Options;
using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Services;
using MunicipiosIbge.Api.Infrastructure.Persistence.Context;
using MunicipiosIbge.Api.Infrastructure.Persistence.Options;
using MunicipiosIbge.Api.Infrastructure.Persistence.Repositories;
using MunicipiosIbge.Api.Infrastructure.Persistence.Repositories.Interfaces;

namespace MunicipiosIbge.Api.Common.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MunicipalitiesDatabase")
            ?? throw new InvalidOperationException("Connection string 'MunicipalitiesDatabase' was not found.");

        services.AddDbContext<MunicipalitiesDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));

        services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.SectionName));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Cache")
                ?? throw new InvalidOperationException("Connection string 'Cache' was not found.");
            options.InstanceName = "MunicipiosIbge:";
        });

        services.AddScoped<IMunicipalityCacheService, MunicipalityCacheService>();
        services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
        services.AddScoped<IMunicipalitySyncService, MunicipalitySyncService>();
        services.AddScoped<IRetrieveMunicipalitiesBehavior, RetrieveMunicipalitiesBehavior>();
        services.AddScoped<IMediator, InMemoryMediator>();
        services.AddScoped<IRequestHandler<GetMunicipalitiesQuery, GetMunicipalitiesResponse>, GetMunicipalitiesHandler>();
        services.AddScoped<IRequestHandler<GetMunicipalityByIdQuery, GetMunicipalityByIdResponse>, GetMunicipalityByIdHandler>();
        services.AddScoped<IRequestHandler<GetMunicipalitiesByMesorregionQuery, GetMunicipalitiesByMesorregionResponse>, GetMunicipalitiesByMesorregionHandler>();
        services.AddScoped<IRequestHandler<SyncMunicipalitiesCommand, SyncMunicipalitiesResponse>, SyncMunicipalitiesHandler>();

        var ibgeOptions = configuration.GetSection(IbgeOptions.SectionName).Get<IbgeOptions>()
            ?? throw new InvalidOperationException("Configuration section 'Ibge' was not found.");

        if (string.IsNullOrWhiteSpace(ibgeOptions.BaseUrl))
        {
            throw new InvalidOperationException("Configuration 'Ibge:BaseUrl' was not found.");
        }

        services.Configure<IbgeOptions>(configuration.GetSection(IbgeOptions.SectionName));

        services.AddHttpClient<IIbgeMunicipalityApi, IbgeMunicipalityService>(client =>
        {
            client.BaseAddress = new Uri(ibgeOptions.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(ibgeOptions.TimeoutSeconds);
        });

        return services;
    }
}
