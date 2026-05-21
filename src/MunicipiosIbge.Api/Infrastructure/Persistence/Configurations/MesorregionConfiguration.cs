using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Configurations;

public sealed class MesorregionConfiguration : IEntityTypeConfiguration<Mesorregion>
{
    public void Configure(EntityTypeBuilder<Mesorregion> builder)
    {
        builder.ToTable("Mesorregions");

        builder.HasKey(mesorregion => mesorregion.Id);

        builder.Property(mesorregion => mesorregion.Id)
            .ValueGeneratedNever();

        builder.Property(mesorregion => mesorregion.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasOne(mesorregion => mesorregion.State)
            .WithMany(state => state.Mesorregions)
            .HasForeignKey(mesorregion => mesorregion.StateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(mesorregion => mesorregion.StateId);

        builder.ConfigureAuditColumns();
    }
}
