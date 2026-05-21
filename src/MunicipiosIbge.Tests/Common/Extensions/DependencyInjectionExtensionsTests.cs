using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MunicipiosIbge.Api.Common.Extensions;
using MunicipiosIbge.Api.Common.Mediator;
using MunicipiosIbge.Api.Infrastructure.Cache.Interfaces;
using MunicipiosIbge.Api.Infrastructure.Persistence.Context;
using MunicipiosIbge.Api.Infrastructure.Persistence.Repositories.Interfaces;

namespace MunicipiosIbge.Tests.Common.Extensions;

public sealed class DependencyInjectionExtensionsTests
{
    [Fact]
    public void AddInfrastructure_WhenConfigurationIsValid_RegistersCoreServices()
    {
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:MunicipalitiesDatabase"] = "Host=localhost;Port=5432;Database=test;Username=test;Password=test",
            ["ConnectionStrings:Cache"] = "localhost:6379",
            ["Ibge:BaseUrl"] = "https://servicodados.ibge.gov.br/",
            ["Ibge:TimeoutSeconds"] = "60"
        });

        services.AddInfrastructure(configuration);

        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(MunicipalitiesDbContext));
        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(IMunicipalityRepository));
        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(IMunicipalityCacheService));
        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(IMediator));
    }

    [Fact]
    public void AddInfrastructure_WhenDatabaseConnectionStringIsMissing_ThrowsInvalidOperationException()
    {
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:Cache"] = "localhost:6379",
            ["Ibge:BaseUrl"] = "https://servicodados.ibge.gov.br/"
        });

        var exception = Record.Exception(() => services.AddInfrastructure(configuration));

        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void AddInfrastructure_WhenIbgeBaseUrlIsMissing_ThrowsInvalidOperationException()
    {
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["ConnectionStrings:MunicipalitiesDatabase"] = "Host=localhost;Port=5432;Database=test;Username=test;Password=test",
            ["ConnectionStrings:Cache"] = "localhost:6379"
        });

        var exception = Record.Exception(() => services.AddInfrastructure(configuration));

        Assert.IsType<InvalidOperationException>(exception);
    }

    private static IConfiguration CreateConfiguration(Dictionary<string, string?> values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }
}
