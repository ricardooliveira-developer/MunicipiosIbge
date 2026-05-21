using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Models;

namespace MunicipiosIbge.Tests.Common.Builders;

public static class IbgeMunicipalityBuilder
{
    public static IbgeMunicipalityResponse Create(
        int id = 1,
        string name = "Sorriso",
        bool withMicroregion = true,
        bool withImmediateRegion = true)
    {
        var region = new IbgeRegionResponse
        {
            Id = 5,
            Acronym = "CO",
            Name = "Centro-Oeste"
        };
        var state = new IbgeStateResponse
        {
            Id = 51,
            Acronym = "MT",
            Name = "Mato Grosso",
            Region = region
        };
        var mesorregion = new IbgeMesorregionResponse
        {
            Id = 5102,
            Name = "Nordeste Mato-grossense",
            State = state
        };
        var microregion = withMicroregion
            ? new IbgeMicroregionResponse
            {
                Id = 51009,
                Name = "Norte Araguaia",
                Mesorregion = mesorregion
            }
            : null;
        var intermediateRegion = new IbgeIntermediateRegionResponse
        {
            Id = 5103,
            Name = "Sinop",
            State = state
        };
        var immediateRegion = withImmediateRegion
            ? new IbgeImmediateRegionResponse
            {
                Id = 510008,
                Name = "Sorriso",
                IntermediateRegion = intermediateRegion
            }
            : null;

        return new IbgeMunicipalityResponse
        {
            Id = id,
            Name = name,
            Microregion = microregion!,
            ImmediateRegion = immediateRegion!
        };
    }
}
