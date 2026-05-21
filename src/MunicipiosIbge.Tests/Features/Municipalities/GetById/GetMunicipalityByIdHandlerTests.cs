using Microsoft.Extensions.Logging.Abstractions;
using MunicipiosIbge.Api.Common.Exceptions;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Handlers;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Models;
using MunicipiosIbge.Tests.Common.Builders;
using MunicipiosIbge.Tests.Common.Fakes;

namespace MunicipiosIbge.Tests.Features.Municipalities.GetById;

public sealed class GetMunicipalityByIdHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenMunicipalityExists_ReturnsMunicipality()
    {
        var behavior = new FakeRetrieveMunicipalitiesBehavior([
            MunicipalityBuilder.Create(id: 5101837, name: "Boa Esperanca do Norte", microregionId: null)
        ]);
        var handler = CreateHandler(behavior);

        var result = await handler.HandleAsync(new GetMunicipalityByIdQuery(5101837));

        Assert.Equal(5101837, result.Municipality.Id);
        Assert.Equal("Boa Esperanca do Norte", result.Municipality.Name);
        Assert.Null(result.Municipality.MicroregionId);
        Assert.Equal("MT", result.Municipality.Uf);
    }

    [Fact]
    public async Task HandleAsync_WhenMunicipalityDoesNotExist_ThrowsNotFoundException()
    {
        var behavior = new FakeRetrieveMunicipalitiesBehavior([
            MunicipalityBuilder.Create(id: 1)
        ]);
        var handler = CreateHandler(behavior);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.HandleAsync(new GetMunicipalityByIdQuery(999)));
    }

    private static GetMunicipalityByIdHandler CreateHandler(FakeRetrieveMunicipalitiesBehavior behavior)
    {
        return new GetMunicipalityByIdHandler(
            behavior,
            NullLogger<GetMunicipalityByIdHandler>.Instance);
    }
}
