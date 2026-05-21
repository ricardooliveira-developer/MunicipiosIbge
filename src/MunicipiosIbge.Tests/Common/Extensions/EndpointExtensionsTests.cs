using Microsoft.AspNetCore.Builder;
using MunicipiosIbge.Api.Common.Extensions;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Endpoints;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Endpoints;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Endpoints;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Endpoints;

namespace MunicipiosIbge.Tests.Common.Extensions;

public sealed class EndpointExtensionsTests
{
    [Fact]
    public void MapApplicationEndpoints_RegistersMunicipalityEndpoints()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        var result = app.MapApplicationEndpoints();

        Assert.Same(app, result);
    }

    [Fact]
    public void IndividualEndpointExtensions_ReturnRouteBuilder()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        Assert.Same(app, app.MapGetMunicipalitiesEndpoint());
        Assert.Same(app, app.MapGetMunicipalityByIdEndpoint());
        Assert.Same(app, app.MapGetMunicipalitiesByMesorregionEndpoint());
        Assert.Same(app, app.MapSyncMunicipalitiesEndpoint());
    }
}
