using Microsoft.EntityFrameworkCore;
using MunicipiosIbge.Api.Domain.Entities;
using MunicipiosIbge.Api.Infrastructure.Persistence.Context;
using MunicipiosIbge.Api.Infrastructure.Persistence.Repositories.Interfaces;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Repositories;

public sealed class MunicipalityRepository(
    MunicipalitiesDbContext dbContext,
    ILogger<MunicipalityRepository> logger) : IMunicipalityRepository
{
    public async Task<IReadOnlyList<Municipality>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await MunicipalityQuery()
            .AsNoTracking()
            .OrderBy(municipality => municipality.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Municipality?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await MunicipalityQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(municipality => municipality.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Municipality>> GetByMesorregionIdAsync(
        int mesorregionId,
        CancellationToken cancellationToken = default)
    {
        return await MunicipalityQuery()
            .AsNoTracking()
            .Where(municipality => municipality.Microregion != null
                && municipality.Microregion.MesorregionId == mesorregionId)
            .OrderBy(municipality => municipality.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Municipality> municipalities, CancellationToken cancellationToken = default)
    {
        await dbContext.Municipalities.AddRangeAsync(municipalities, cancellationToken);
    }

    public async Task ReplaceAllAsync(
        IEnumerable<Region> regions,
        IEnumerable<State> states,
        IEnumerable<Mesorregion> mesorregions,
        IEnumerable<Microregion> microregions,
        IEnumerable<IntermediateRegion> intermediateRegions,
        IEnumerable<ImmediateRegion> immediateRegions,
        IEnumerable<Municipality> municipalities,
        CancellationToken cancellationToken = default)
    {
        var regionList = regions.ToList();
        var stateList = states.ToList();
        var mesorregionList = mesorregions.ToList();
        var microregionList = microregions.ToList();
        var intermediateRegionList = intermediateRegions.ToList();
        var immediateRegionList = immediateRegions.ToList();
        var municipalityList = municipalities.ToList();

        logger.LogInformation(
            "Atualização do banco iniciada: preparando para substituir todos os dados por {MunicipalityCount} municípios e suas hierarquias ({RegionCount} regiões, {StateCount} UFs, {MesorregionCount} mesorregiões, {MicroregionCount} microrregiões, {IntermediateRegionCount} regiões intermediárias e {ImmediateRegionCount} regiões imediatas).",
            regionList.Count,
            stateList.Count,
            mesorregionList.Count,
            microregionList.Count,
            intermediateRegionList.Count,
            immediateRegionList.Count,
            municipalityList.Count);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        logger.LogInformation("Limpando dados antigos do banco antes de inserir o novo snapshot.");
        await dbContext.Municipalities.ExecuteDeleteAsync(cancellationToken);
        await dbContext.ImmediateRegions.ExecuteDeleteAsync(cancellationToken);
        await dbContext.Microregions.ExecuteDeleteAsync(cancellationToken);
        await dbContext.IntermediateRegions.ExecuteDeleteAsync(cancellationToken);
        await dbContext.Mesorregions.ExecuteDeleteAsync(cancellationToken);
        await dbContext.States.ExecuteDeleteAsync(cancellationToken);
        await dbContext.Regions.ExecuteDeleteAsync(cancellationToken);

        logger.LogInformation("Inserindo novo snapshot de municípios e hierarquias no banco.");
        dbContext.Regions.AddRange(regionList);
        dbContext.States.AddRange(stateList);
        dbContext.Mesorregions.AddRange(mesorregionList);
        dbContext.Microregions.AddRange(microregionList);
        dbContext.IntermediateRegions.AddRange(intermediateRegionList);
        dbContext.ImmediateRegions.AddRange(immediateRegionList);
        await AddRangeAsync(municipalityList, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        logger.LogInformation(
            "Atualização do banco concluída com sucesso: transação confirmada com {MunicipalityCount} municípios.",
            municipalityList.Count);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<Municipality> MunicipalityQuery()
    {
        return dbContext.Municipalities
            .Include(municipality => municipality.Microregion)
                .ThenInclude(microregion => microregion!.Mesorregion)
                    .ThenInclude(mesorregion => mesorregion.State)
                        .ThenInclude(state => state.Region)
            .Include(municipality => municipality.ImmediateRegion)
                .ThenInclude(immediateRegion => immediateRegion.IntermediateRegion)
                    .ThenInclude(intermediateRegion => intermediateRegion.State)
                        .ThenInclude(state => state.Region);
    }
}
