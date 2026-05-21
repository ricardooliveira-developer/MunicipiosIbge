using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Configurations;

public sealed class IntermediateRegionConfiguration : IEntityTypeConfiguration<IntermediateRegion>
{
    public void Configure(EntityTypeBuilder<IntermediateRegion> builder)
    {
        builder.ToTable("IntermediateRegions");

        builder.HasKey(intermediateRegion => intermediateRegion.Id);

        builder.Property(intermediateRegion => intermediateRegion.Id)
            .ValueGeneratedNever();

        builder.Property(intermediateRegion => intermediateRegion.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasOne(intermediateRegion => intermediateRegion.State)
            .WithMany(state => state.IntermediateRegions)
            .HasForeignKey(intermediateRegion => intermediateRegion.StateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(intermediateRegion => intermediateRegion.StateId);

        builder.ConfigureAuditColumns();
    }
}
