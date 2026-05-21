using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Configurations;

internal static class AuditableEntityConfigurationExtensions
{
    public static void ConfigureAuditColumns<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseAuditableEntity
    {
        builder.Property(entity => entity.CreatedAt)
            .IsRequired();

        builder.Property(entity => entity.UpdatedAt);
    }
}
