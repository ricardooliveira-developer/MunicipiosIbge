using System.Reflection;
using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Tests.Common.Builders;

public static class MunicipalityBuilder
{
    public static Municipality Create(
        int id = 1,
        string name = "Sorriso",
        string uf = "MT",
        int stateId = 51,
        int regionId = 5,
        int mesorregionId = 5102,
        int? microregionId = 51009,
        int intermediateRegionId = 5103,
        int immediateRegionId = 510008)
    {
        var region = new Region(regionId, "CO", "Centro-Oeste");
        var state = new State(stateId, uf, "Mato Grosso", regionId);
        SetNavigation(state, nameof(State.Region), region);

        Microregion? microregion = null;

        if (microregionId.HasValue)
        {
            var mesorregion = new Mesorregion(mesorregionId, "Nordeste Mato-grossense", stateId);
            SetNavigation(mesorregion, nameof(Mesorregion.State), state);

            microregion = new Microregion(microregionId.Value, "Norte Araguaia", mesorregionId);
            SetNavigation(microregion, nameof(Microregion.Mesorregion), mesorregion);
        }

        var intermediateRegion = new IntermediateRegion(intermediateRegionId, "Sinop", stateId);
        SetNavigation(intermediateRegion, nameof(IntermediateRegion.State), state);

        var immediateRegion = new ImmediateRegion(immediateRegionId, "Sorriso", intermediateRegionId);
        SetNavigation(immediateRegion, nameof(ImmediateRegion.IntermediateRegion), intermediateRegion);

        var municipality = new Municipality(id, name, microregionId, immediateRegionId);
        SetNavigation(municipality, nameof(Municipality.Microregion), microregion);
        SetNavigation(municipality, nameof(Municipality.ImmediateRegion), immediateRegion);

        return municipality;
    }

    private static void SetNavigation<TTarget, TValue>(TTarget target, string propertyName, TValue value)
        where TTarget : class
    {
        typeof(TTarget)
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
            .SetValue(target, value);
    }
}
