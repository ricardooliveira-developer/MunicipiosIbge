using MunicipiosIbge.Api.Domain.Entities;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Models;
using MunicipiosIbge.Api.Infrastructure.Cache.Interfaces;
using MunicipiosIbge.Api.Infrastructure.Cache.Models;
using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Clients;
using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Models;
using MunicipiosIbge.Api.Infrastructure.Persistence.Repositories.Interfaces;

namespace MunicipiosIbge.Api.Features.Municipalities.Sync.Services;

public sealed class MunicipalitySyncService(
    IIbgeMunicipalityApi ibgeMunicipalityApi,
    IMunicipalityRepository municipalityRepository,
    IMunicipalityCacheService cacheService,
    ILogger<MunicipalitySyncService> logger) : IMunicipalitySyncService
{
    public async Task<SyncMunicipalitiesResponse> SyncAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Sincronização iniciada: buscando municípios no IBGE.");

        var ibgeMunicipalities = await ibgeMunicipalityApi.GetMunicipalitiesAsync(cancellationToken);

        if (ibgeMunicipalities.Count == 0)
        {
            throw new InvalidOperationException("IBGE returned no municipalities to synchronize.");
        }

        var currentMunicipalities = await municipalityRepository.GetAllAsync(cancellationToken);
        var snapshot = BuildSnapshot(ibgeMunicipalities);

        logger.LogInformation(
            "Dados recebidos do IBGE: {TotalReceived} municípios. O banco será substituído por um novo snapshot. Hoje existem {CurrentCount} municípios no banco. Novo snapshot: {RegionCount} regiões, {StateCount} UFs, {MesorregionCount} mesorregiões, {MicroregionCount} microrregiões, {IntermediateRegionCount} regiões intermediárias, {ImmediateRegionCount} regiões imediatas e {MunicipalityCount} municípios.",
            ibgeMunicipalities.Count,
            currentMunicipalities.Count,
            snapshot.Regions.Count,
            snapshot.States.Count,
            snapshot.Mesorregions.Count,
            snapshot.Microregions.Count,
            snapshot.IntermediateRegions.Count,
            snapshot.ImmediateRegions.Count,
            snapshot.Municipalities.Count);

        await municipalityRepository.ReplaceAllAsync(
            snapshot.Regions.Values,
            snapshot.States.Values,
            snapshot.Mesorregions.Values,
            snapshot.Microregions.Values,
            snapshot.IntermediateRegions.Values,
            snapshot.ImmediateRegions.Values,
            snapshot.Municipalities.Values,
            cancellationToken);

        logger.LogInformation("Banco atualizado com o novo snapshot. Recriando o cache dos municípios.");

        await RemovePreviousCacheKeysAsync(currentMunicipalities, cancellationToken);
        var cachedKeys = await RefreshCacheAsync(cancellationToken);

        logger.LogInformation(
            "Sincronização finalizada: {DeletedCount} municípios antigos removidos, {InsertedCount} municípios inseridos e {CachedKeys} chaves gravadas no cache.",
            currentMunicipalities.Count,
            snapshot.Municipalities.Count,
            cachedKeys);

        return new SyncMunicipalitiesResponse(
            ibgeMunicipalities.Count,
            currentMunicipalities.Count,
            snapshot.Municipalities.Count,
            cachedKeys);
    }

    private static MunicipalitySnapshot BuildSnapshot(IReadOnlyList<IbgeMunicipalityResponse> ibgeMunicipalities)
    {
        var snapshot = new MunicipalitySnapshot();

        foreach (var ibgeMunicipality in ibgeMunicipalities)
        {
            ValidateMunicipalityPayload(ibgeMunicipality);

            if (ibgeMunicipality.Microregion?.Mesorregion?.State?.Region is not null)
            {
                AddStateHierarchy(snapshot, ibgeMunicipality.Microregion.Mesorregion.State);

                snapshot.Mesorregions.TryAdd(
                    ibgeMunicipality.Microregion.Mesorregion.Id,
                    new Mesorregion(
                        ibgeMunicipality.Microregion.Mesorregion.Id,
                        ibgeMunicipality.Microregion.Mesorregion.Name,
                        ibgeMunicipality.Microregion.Mesorregion.State.Id));

                snapshot.Microregions.TryAdd(
                    ibgeMunicipality.Microregion.Id,
                    new Microregion(
                        ibgeMunicipality.Microregion.Id,
                        ibgeMunicipality.Microregion.Name,
                        ibgeMunicipality.Microregion.Mesorregion.Id));
            }

            AddStateHierarchy(snapshot, ibgeMunicipality.ImmediateRegion.IntermediateRegion.State);

            snapshot.IntermediateRegions.TryAdd(
                ibgeMunicipality.ImmediateRegion.IntermediateRegion.Id,
                new IntermediateRegion(
                    ibgeMunicipality.ImmediateRegion.IntermediateRegion.Id,
                    ibgeMunicipality.ImmediateRegion.IntermediateRegion.Name,
                    ibgeMunicipality.ImmediateRegion.IntermediateRegion.State.Id));

            snapshot.ImmediateRegions.TryAdd(
                ibgeMunicipality.ImmediateRegion.Id,
                new ImmediateRegion(
                    ibgeMunicipality.ImmediateRegion.Id,
                    ibgeMunicipality.ImmediateRegion.Name,
                    ibgeMunicipality.ImmediateRegion.IntermediateRegion.Id));

            snapshot.Municipalities.TryAdd(
                ibgeMunicipality.Id,
                new Municipality(
                    ibgeMunicipality.Id,
                    ibgeMunicipality.Name,
                    ibgeMunicipality.Microregion?.Id,
                    ibgeMunicipality.ImmediateRegion.Id));
        }

        return snapshot;
    }

    private static void ValidateMunicipalityPayload(IbgeMunicipalityResponse municipality)
    {
        if (municipality.ImmediateRegion?.IntermediateRegion?.State?.Region is null)
        {
            throw new InvalidOperationException(
                $"IBGE returned municipality '{municipality.Id}' without the complete immediate region hierarchy.");
        }
    }

    private static void AddStateHierarchy(MunicipalitySnapshot snapshot, IbgeStateResponse ibgeState)
    {
        snapshot.Regions.TryAdd(
            ibgeState.Region.Id,
            new Region(ibgeState.Region.Id, ibgeState.Region.Acronym, ibgeState.Region.Name));

        snapshot.States.TryAdd(
            ibgeState.Id,
            new State(ibgeState.Id, ibgeState.Acronym, ibgeState.Name, ibgeState.Region.Id));
    }

    private async Task RemovePreviousCacheKeysAsync(
        IReadOnlyList<Municipality> currentMunicipalities,
        CancellationToken cancellationToken)
    {
        await cacheService.RemoveAsync(MunicipalityCacheKey.All(), cancellationToken);
        var removedKeys = 1;

        foreach (var municipality in currentMunicipalities)
        {
            await cacheService.RemoveAsync(MunicipalityCacheKey.ById(municipality.Id), cancellationToken);
            removedKeys++;
        }

        foreach (var mesorregionId in currentMunicipalities
            .Where(municipality => municipality.Microregion is not null)
            .Select(municipality => municipality.Microregion!.MesorregionId)
            .Distinct())
        {
            await cacheService.RemoveAsync(MunicipalityCacheKey.ByMesorregion(mesorregionId), cancellationToken);
            removedKeys++;
        }

        logger.LogInformation("Cache antigo removido: {RemovedCacheKeys} chaves de municípios foram apagadas.", removedKeys);
    }

    private async Task<int> RefreshCacheAsync(CancellationToken cancellationToken)
    {
        var municipalities = await municipalityRepository.GetAllAsync(cancellationToken);
        var cachedKeys = 0;

        await cacheService.SetAsync(MunicipalityCacheKey.All(), municipalities, cancellationToken: cancellationToken);
        cachedKeys++;

        foreach (var municipality in municipalities)
        {
            await cacheService.SetAsync(
                MunicipalityCacheKey.ById(municipality.Id),
                municipality,
                cancellationToken: cancellationToken);
            cachedKeys++;
        }

        foreach (var group in municipalities
            .Where(municipality => municipality.Microregion is not null)
            .GroupBy(municipality => municipality.Microregion!.MesorregionId))
        {
            await cacheService.SetAsync(
                MunicipalityCacheKey.ByMesorregion(group.Key),
                group.ToList(),
                cancellationToken: cancellationToken);
            cachedKeys++;
        }

        logger.LogInformation("Cache recriado: {CachedKeys} chaves de municípios foram gravadas.", cachedKeys);

        return cachedKeys;
    }

    private sealed class MunicipalitySnapshot
    {
        public Dictionary<int, Region> Regions { get; } = [];
        public Dictionary<int, State> States { get; } = [];
        public Dictionary<int, Mesorregion> Mesorregions { get; } = [];
        public Dictionary<int, Microregion> Microregions { get; } = [];
        public Dictionary<int, IntermediateRegion> IntermediateRegions { get; } = [];
        public Dictionary<int, ImmediateRegion> ImmediateRegions { get; } = [];
        public Dictionary<int, Municipality> Municipalities { get; } = [];
    }
}
