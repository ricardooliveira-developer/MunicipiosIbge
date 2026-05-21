namespace MunicipiosIbge.Api.Features.Municipalities.Sync.Docs;

public static class SyncMunicipalitiesEndpointDocs
{
    public const string Summary = "Sincroniza municípios com a API pública do IBGE";

    public const string Description = """
    Busca todos os municípios na API pública do IBGE e recria o snapshot local.

    O processo executa as seguintes etapas:

    1. Chama `https://servicodados.ibge.gov.br/api/v1/localidades/municipios`.
    2. Monta o snapshot normalizado de regiões, UFs, mesorregiões, microrregiões, regiões intermediárias, regiões imediatas e municípios.
    3. Remove os dados antigos do PostgreSQL em uma transação.
    4. Insere o novo snapshot.
    5. Recria o cache Redis.

    Exemplo de sucesso:

    ```json
    {
      "success": true,
      "data": {
        "totalReceived": 5571,
        "municipalitiesDeleted": 5570,
        "municipalitiesInserted": 5571,
        "cachedKeys": 5709
      },
      "message": "Municipalities synchronized successfully."
    }
    ```

    Exemplo de erro externo:

    ```json
    {
      "success": false,
      "message": "IBGE service returned 500 (InternalServerError)."
    }
    ```
    """;
}
