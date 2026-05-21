namespace MunicipiosIbge.Api.Features.Municipalities.GetAll.Docs;

public static class GetMunicipalitiesEndpointDocs
{
    public const string Summary = "Lista municípios com filtros opcionais";

    public const string Description = """
    Retorna municípios em um formato simplificado para consumo da aplicação.

    O fluxo é cache-first:

    1. Busca a lista completa no Redis.
    2. Se não houver cache válido, busca no PostgreSQL.
    3. Se o banco estiver vazio, sincroniza com a API pública do IBGE.

    A paginação é opcional. Quando `page` e `pageSize` não são informados, todos os registros encontrados são retornados.

    Filtros disponíveis:

    - `name`: busca parcial pelo nome do município.
    - `UF`: sigla da unidade federativa, como `MT`, `SP`, `RJ`.
    - `stateAcronym`: alias legado para `UF`.
    - `stateId`: código IBGE da UF.
    - `regionId` / `regionAcronym`: filtros por grande região.
    - `mesorregionId`, `microregionId`, `intermediateRegionId`, `immediateRegionId`: filtros territoriais.

    Exemplo de sucesso:

    ```json
    {
      "success": true,
      "data": {
        "items": [
          {
            "id": 5101837,
            "name": "Boa Esperança do Norte",
            "UF": "MT",
            "stateId": 51,
            "stateName": "Mato Grosso",
            "regionId": 5,
            "regionAcronym": "CO",
            "regionName": "Centro-Oeste",
            "mesorregionId": null,
            "mesorregionName": null,
            "microregionId": null,
            "microregionName": null,
            "immediateRegionId": 510008,
            "immediateRegionName": "Sorriso",
            "intermediateRegionId": 5103,
            "intermediateRegionName": "Sinop"
          }
        ],
        "page": 1,
        "pageSize": 1,
        "totalItems": 1,
        "totalPages": 1
      },
      "message": null
    }
    ```

    Exemplo de erro de validação:

    ```json
    {
      "success": false,
      "message": "PageSize must be between 1 and 200."
    }
    ```
    """;
}
