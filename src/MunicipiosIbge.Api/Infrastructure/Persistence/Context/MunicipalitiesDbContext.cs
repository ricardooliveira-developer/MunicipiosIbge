using Microsoft.EntityFrameworkCore;
using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Context;

public sealed class MunicipalitiesDbContext(DbContextOptions<MunicipalitiesDbContext> options) : DbContext(options)
{
    public DbSet<Region> Regions => Set<Region>();
    public DbSet<State> States => Set<State>();
    public DbSet<Mesorregion> Mesorregions => Set<Mesorregion>();
    public DbSet<Microregion> Microregions => Set<Microregion>();
    public DbSet<IntermediateRegion> IntermediateRegions => Set<IntermediateRegion>();
    public DbSet<ImmediateRegion> ImmediateRegions => Set<ImmediateRegion>();
    public DbSet<Municipality> Municipalities => Set<Municipality>();

    public override int SaveChanges()
    {
        ApplyAuditInformation();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MunicipalitiesDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    private void ApplyAuditInformation()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.MarkAsCreated(utcNow);
            }

            if (entry.State != EntityState.Modified) continue;
            
            entry.Property(entity => entity.CreatedAt).IsModified = false;
            entry.Entity.MarkAsUpdated(utcNow);
        }
    }
}
