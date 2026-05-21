using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Tests.Domain.Entities;

public sealed class EntityUpdateTests
{
    [Fact]
    public void UpdateMethods_UpdateEntityState()
    {
        var region = new Region(1, "N", "Norte");
        var state = new State(11, "RO", "Rondonia", 1);
        var mesorregion = new Mesorregion(1102, "Leste", 11);
        var microregion = new Microregion(11006, "Cacoal", 1102);
        var intermediateRegion = new IntermediateRegion(1102, "Ji-Parana", 11);
        var immediateRegion = new ImmediateRegion(110005, "Cacoal", 1102);
        var municipality = new Municipality(1100015, "Alta Floresta", 11006, 110005);

        region.Update("CO", "Centro-Oeste");
        state.Update("MT", "Mato Grosso", 5);
        mesorregion.Update("Nordeste", 51);
        microregion.Update("Norte Araguaia", 5102);
        intermediateRegion.Update("Sinop", 51);
        immediateRegion.Update("Sorriso", 5103);
        municipality.Update("Boa Esperanca do Norte", null, 510008);

        Assert.Equal("CO", region.Acronym);
        Assert.Equal("MT", state.Acronym);
        Assert.Equal(51, mesorregion.StateId);
        Assert.Equal(5102, microregion.MesorregionId);
        Assert.Equal(51, intermediateRegion.StateId);
        Assert.Equal(5103, immediateRegion.IntermediateRegionId);
        Assert.Null(municipality.MicroregionId);
        Assert.Equal(510008, municipality.ImmediateRegionId);
    }

    [Fact]
    public void AuditMethods_UpdateAuditProperties()
    {
        var municipality = new Municipality(1, "Sorriso", 51009, 510008);
        var createdAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var updatedAt = createdAt.AddHours(1);

        municipality.MarkAsCreated(createdAt);
        municipality.MarkAsUpdated(updatedAt);

        Assert.Equal(createdAt, municipality.CreatedAt);
        Assert.Equal(updatedAt, municipality.UpdatedAt);
    }
}
