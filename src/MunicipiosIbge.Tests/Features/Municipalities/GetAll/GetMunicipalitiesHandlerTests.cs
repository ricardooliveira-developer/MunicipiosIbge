using Microsoft.Extensions.Logging.Abstractions;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Handlers;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;
using MunicipiosIbge.Tests.Common.Builders;
using MunicipiosIbge.Tests.Common.Fakes;

namespace MunicipiosIbge.Tests.Features.Municipalities.GetAll;

public sealed class GetMunicipalitiesHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenPaginationIsNotProvided_ReturnsAllItems()
    {
        var behavior = new FakeRetrieveMunicipalitiesBehavior([
            MunicipalityBuilder.Create(id: 1, name: "Sorriso"),
            MunicipalityBuilder.Create(id: 2, name: "Cuiaba")
        ]);
        var handler = CreateHandler(behavior);

        var result = await handler.HandleAsync(new GetMunicipalitiesQuery());

        Assert.Equal(2, result.TotalItems);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(1, result.Page);
        Assert.Equal(2, result.PageSize);
    }

    [Fact]
    public async Task HandleAsync_WhenUfFilterIsProvided_ReturnsOnlyMatchingUf()
    {
        var behavior = new FakeRetrieveMunicipalitiesBehavior([
            MunicipalityBuilder.Create(id: 1, name: "Sorriso", uf: "MT", stateId: 51),
            MunicipalityBuilder.Create(id: 2, name: "Porto Velho", uf: "RO", stateId: 11, regionId: 1)
        ]);
        var handler = CreateHandler(behavior);

        var result = await handler.HandleAsync(new GetMunicipalitiesQuery { Uf = "MT" });

        var item = Assert.Single(result.Items);
        Assert.Equal("MT", item.Uf);
        Assert.Equal(51, item.StateId);
    }

    [Fact]
    public async Task HandleAsync_WhenPaginationIsProvided_ReturnsRequestedPage()
    {
        var behavior = new FakeRetrieveMunicipalitiesBehavior([
            MunicipalityBuilder.Create(id: 1, name: "A"),
            MunicipalityBuilder.Create(id: 2, name: "B"),
            MunicipalityBuilder.Create(id: 3, name: "C")
        ]);
        var handler = CreateHandler(behavior);

        var result = await handler.HandleAsync(new GetMunicipalitiesQuery { Page = 2, PageSize = 1 });

        var item = Assert.Single(result.Items);
        Assert.Equal(2, result.Page);
        Assert.Equal(1, result.PageSize);
        Assert.Equal(3, result.TotalItems);
        Assert.Equal(3, result.TotalPages);
        Assert.Equal("B", item.Name);
    }

    private static GetMunicipalitiesHandler CreateHandler(FakeRetrieveMunicipalitiesBehavior behavior)
    {
        return new GetMunicipalitiesHandler(
            behavior,
            NullLogger<GetMunicipalitiesHandler>.Instance);
    }
}
