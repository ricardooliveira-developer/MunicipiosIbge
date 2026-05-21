using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Configurations;

public sealed class MicroregionConfiguration : IEntityTypeConfiguration<Microregion>
{
    public void Configure(EntityTypeBuilder<Microregion> builder)
    {
        builder.ToTable("Microregions");

        builder.HasKey(microregion => microregion.Id);

        builder.Property(microregion => microregion.Id)
            .ValueGeneratedNever();

        builder.Property(microregion => microregion.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasOne(microregion => microregion.Mesorregion)
            .WithMany(mesorregion => mesorregion.Microregions)
            .HasForeignKey(microregion => microregion.MesorregionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(microregion => microregion.MesorregionId);

        builder.ConfigureAuditColumns();
    }
}
