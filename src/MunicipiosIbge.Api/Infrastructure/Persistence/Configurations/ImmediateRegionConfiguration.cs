using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Configurations;

public sealed class ImmediateRegionConfiguration : IEntityTypeConfiguration<ImmediateRegion>
{
    public void Configure(EntityTypeBuilder<ImmediateRegion> builder)
    {
        builder.ToTable("ImmediateRegions");

        builder.HasKey(immediateRegion => immediateRegion.Id);

        builder.Property(immediateRegion => immediateRegion.Id)
            .ValueGeneratedNever();

        builder.Property(immediateRegion => immediateRegion.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasOne(immediateRegion => immediateRegion.IntermediateRegion)
            .WithMany(intermediateRegion => intermediateRegion.ImmediateRegions)
            .HasForeignKey(immediateRegion => immediateRegion.IntermediateRegionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(immediateRegion => immediateRegion.IntermediateRegionId);

        builder.ConfigureAuditColumns();
    }
}
