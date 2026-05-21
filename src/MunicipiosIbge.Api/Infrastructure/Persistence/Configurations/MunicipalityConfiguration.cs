using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Configurations;

public sealed class MunicipalityConfiguration : IEntityTypeConfiguration<Municipality>
{
    public void Configure(EntityTypeBuilder<Municipality> builder)
    {
        builder.ToTable("Municipalities");

        builder.HasKey(municipality => municipality.Id);

        builder.Property(municipality => municipality.Id)
            .ValueGeneratedNever();

        builder.Property(municipality => municipality.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasOne(municipality => municipality.Microregion)
            .WithMany(microregion => microregion.Municipalities)
            .HasForeignKey(municipality => municipality.MicroregionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(municipality => municipality.ImmediateRegion)
            .WithMany(immediateRegion => immediateRegion.Municipalities)
            .HasForeignKey(municipality => municipality.ImmediateRegionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(municipality => municipality.Name);
        builder.HasIndex(municipality => municipality.MicroregionId);
        builder.HasIndex(municipality => municipality.ImmediateRegionId);

        builder.ConfigureAuditColumns();
    }
}
