using MunicipiosIbge.Api.Domain.Entities;
using MunicipiosIbge.Api.Features.Municipalities.Sync.Services;
using MunicipiosIbge.Api.Infrastructure.Cache.Interfaces;
using MunicipiosIbge.Api.Infrastructure.Cache.Models;
using MunicipiosIbge.Api.Infrastructure.Persistence.Repositories.Interfaces;

namespace MunicipiosIbge.Api.Common.Behaviors;

public sealed class RetrieveMunicipalitiesBehavior(
    IMunicipalityCacheService cacheService,
    IMunicipalityRepository municipalityRepository,
    IMunicipalitySyncService municipalitySyncService,
    ILogger<RetrieveMunicipalitiesBehavior> logger) : IRetrieveMunicipalitiesBehavior
{
    public async Task<IReadOnlyList<Municipality>> RetrieveAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Buscando municípios: tentando ler a lista no cache.");

        var cachedMunicipalities = await cacheService.GetAsync<List<Municipality>>(
            MunicipalityCacheKey.All(),
            cancellationToken);

        if (IsValidCachedGraph(cachedMunicipalities))
        {
            logger.LogInformation(
                "Cache encontrado: usando {MunicipalityCount} municípios já armazenados.",
                cachedMunicipalities!.Count);

            return cachedMunicipalities!;
        }

        if (cachedMunicipalities is { Count: > 0 })
        {
            logger.LogWarning(
                "Cache encontrado, mas incompleto: {MunicipalityCount} municípios estavam salvos sem toda a hierarquia. A chave {CacheKey} será removida.",
                cachedMunicipalities.Count,
                MunicipalityCacheKey.All());

            await cacheService.RemoveAsync(MunicipalityCacheKey.All(), cancellationToken);
        }

        logger.LogInformation("Cache vazio: buscando municípios no banco de dados.");
        var storedMunicipalities = await municipalityRepository.GetAllAsync(cancellationToken);

        if (storedMunicipalities.Count > 0)
        {
            logger.LogInformation(
                "Banco de dados encontrado: {MunicipalityCount} municípios carregados. Atualizando a chave de cache {CacheKey}.",
                storedMunicipalities.Count,
                MunicipalityCacheKey.All());

            await cacheService.SetAsync(
                MunicipalityCacheKey.All(),
                storedMunicipalities,
                cancellationToken: cancellationToken);

            return storedMunicipalities;
        }

        logger.LogInformation("Banco de dados vazio: iniciando sincronização com a API do IBGE.");
        await municipalitySyncService.SyncAsync(cancellationToken);

        cachedMunicipalities = await cacheService.GetAsync<List<Municipality>>(
            MunicipalityCacheKey.All(),
            cancellationToken);

        if (IsValidCachedGraph(cachedMunicipalities))
        {
            logger.LogInformation(
                "Sincronização concluída: {MunicipalityCount} municípios carregados a partir do cache atualizado.",
                cachedMunicipalities!.Count);

            return cachedMunicipalities!;
        }

        logger.LogWarning("Cache ainda vazio após a sincronização. Buscando os municípios diretamente no banco.");
        storedMunicipalities = await municipalityRepository.GetAllAsync(cancellationToken);

        if (storedMunicipalities.Count > 0)
        {
            logger.LogInformation(
                "Banco retornou {MunicipalityCount} municípios após a sincronização. Atualizando o cache.",
                storedMunicipalities.Count);

            await cacheService.SetAsync(
                MunicipalityCacheKey.All(),
                storedMunicipalities,
                cancellationToken: cancellationToken);
        }

        return storedMunicipalities;
    }

    private static bool IsValidCachedGraph(List<Municipality>? municipalities)
    {
        return municipalities is { Count: > 0 }
            && municipalities.All(municipality =>
                municipality.ImmediateRegion?.IntermediateRegion?.State?.Region is not null
                && (municipality.Microregion is null
                    || municipality.Microregion.Mesorregion.State.Region is not null));
    }
}
