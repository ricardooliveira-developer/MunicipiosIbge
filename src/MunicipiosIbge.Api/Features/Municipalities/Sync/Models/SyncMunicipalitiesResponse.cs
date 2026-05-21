namespace MunicipiosIbge.Api.Features.Municipalities.Sync.Models;

/// <summary>
/// Resultado da sincronização de municípios com a API pública do IBGE.
/// </summary>
/// <param name="TotalReceived">Quantidade de municípios recebidos da API do IBGE.</param>
/// <param name="MunicipalitiesDeleted">Quantidade de municípios removidos do snapshot anterior.</param>
/// <param name="MunicipalitiesInserted">Quantidade de municípios inseridos no novo snapshot.</param>
/// <param name="CachedKeys">Quantidade de chaves gravadas no cache Redis.</param>
public sealed record SyncMunicipalitiesResponse(
    int TotalReceived,
    int MunicipalitiesDeleted,
    int MunicipalitiesInserted,
    int CachedKeys);
