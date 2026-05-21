using Microsoft.EntityFrameworkCore;
using MunicipiosIbge.Api.Domain.Entities;
using MunicipiosIbge.Api.Infrastructure.Persistence.Context;

namespace MunicipiosIbge.Tests.Infrastructure.Persistence;

public sealed class MunicipalitiesDbContextTests
{
    [Fact]
    public async Task SaveChangesAsync_WhenEntityIsAdded_SetsCreatedAt()
    {
        await using var context = CreateContext();

        context.Regions.Add(new Region(1, "N", "Norte"));

        await context.SaveChangesAsync();

        var region = await context.Regions.SingleAsync();
        Assert.NotEqual(default, region.CreatedAt);
        Assert.Null(region.UpdatedAt);
    }

    [Fact]
    public async Task SaveChangesAsync_WhenEntityIsModified_SetsUpdatedAtAndKeepsCreatedAt()
    {
        await using var context = CreateContext();
        var region = new Region(1, "N", "Norte");
        context.Regions.Add(region);
        await context.SaveChangesAsync();
        var createdAt = region.CreatedAt;

        region.Update("NO", "Norte Atualizado");
        await context.SaveChangesAsync();

        Assert.Equal(createdAt, region.CreatedAt);
        Assert.NotNull(region.UpdatedAt);
    }

    [Fact]
    public void ModelConfiguration_AllowsMunicipalityWithoutMicroregion()
    {
        using var context = CreateContext();

        var municipality = context.Model.FindEntityType(typeof(Municipality))!;
        var microregionId = municipality.FindProperty(nameof(Municipality.MicroregionId))!;
        var immediateRegionId = municipality.FindProperty(nameof(Municipality.ImmediateRegionId))!;

        Assert.True(microregionId.IsNullable);
        Assert.False(immediateRegionId.IsNullable);
    }

    private static MunicipalitiesDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MunicipalitiesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new MunicipalitiesDbContext(options);
    }
}
