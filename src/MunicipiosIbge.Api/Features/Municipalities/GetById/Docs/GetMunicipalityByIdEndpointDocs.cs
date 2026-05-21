namespace MunicipiosIbge.Api.Features.Municipalities.GetById.Docs;

public static class GetMunicipalityByIdEndpointDocs
{
    public const string Summary = "Busca um município pelo código IBGE";

    public const string Description = """
    Retorna um único município pelo seu código oficial do IBGE.

    O endpoint usa o mesmo fluxo cache-first das demais consultas:

    1. Tenta ler a lista no Redis.
    2. Se não encontrar, busca no PostgreSQL.
    3. Se o banco estiver vazio, sincroniza com a API do IBGE.

    Exemplo:

    ```http
    GET /municipalities/5101837
    ```

    Exemplo de sucesso:

    ```json
    {
      "success": true,
      "data": {
        "municipality": {
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
      },
      "message": null
    }
    ```

    Exemplo de erro 404:

    ```json
    {
      "success": false,
      "message": "Municipality '9999999' was not found."
    }
    ```
    """;
}
