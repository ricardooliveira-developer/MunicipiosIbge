using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Configurations;

public sealed class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.ToTable("Regions");

        builder.HasKey(region => region.Id);

        builder.Property(region => region.Id)
            .ValueGeneratedNever();

        builder.Property(region => region.Acronym)
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(region => region.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(region => region.Acronym)
            .IsUnique();

        builder.ConfigureAuditColumns();
    }
}
