using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Configurations;

public sealed class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.ToTable("States");

        builder.HasKey(state => state.Id);

        builder.Property(state => state.Id)
            .ValueGeneratedNever();

        builder.Property(state => state.Acronym)
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(state => state.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(state => state.Acronym)
            .IsUnique();

        builder.HasOne(state => state.Region)
            .WithMany(region => region.States)
            .HasForeignKey(state => state.RegionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ConfigureAuditColumns();
    }
}
