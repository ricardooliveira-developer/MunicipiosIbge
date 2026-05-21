using Microsoft.Extensions.Logging.Abstractions;
using MunicipiosIbge.Api.Common.Exceptions;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Handlers;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Models;
using MunicipiosIbge.Tests.Common.Builders;
using MunicipiosIbge.Tests.Common.Fakes;

namespace MunicipiosIbge.Tests.Features.Municipalities.GetByMesorregion;

public sealed class GetMunicipalitiesByMesorregionHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenMesorregionHasMunicipalities_ReturnsOnlyMatchingMunicipalities()
    {
        var behavior = new FakeRetrieveMunicipalitiesBehavior([
            MunicipalityBuilder.Create(id: 1, name: "A", mesorregionId: 5102),
            MunicipalityBuilder.Create(id: 2, name: "B", mesorregionId: 1102),
            MunicipalityBuilder.Create(id: 3, name: "C", microregionId: null)
        ]);
        var handler = CreateHandler(behavior);

        var result = await handler.HandleAsync(new GetMunicipalitiesByMesorregionQuery { MesorregionId = 5102 });

        var item = Assert.Single(result.Items);
        Assert.Equal(5102, result.MesorregionId);
        Assert.Equal(5102, item.MesorregionId);
        Assert.Equal("A", item.Name);
    }

    [Fact]
    public async Task HandleAsync_WhenNoMunicipalityMatches_ThrowsNotFoundException()
    {
        var behavior = new FakeRetrieveMunicipalitiesBehavior([
            MunicipalityBuilder.Create(id: 1, mesorregionId: 5102)
        ]);
        var handler = CreateHandler(behavior);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new GetMunicipalitiesByMesorregionQuery { MesorregionId = 9999 }));
    }

    private static GetMunicipalitiesByMesorregionHandler CreateHandler(FakeRetrieveMunicipalitiesBehavior behavior)
    {
        return new GetMunicipalitiesByMesorregionHandler(
            behavior,
            NullLogger<GetMunicipalitiesByMesorregionHandler>.Instance);
    }
}
