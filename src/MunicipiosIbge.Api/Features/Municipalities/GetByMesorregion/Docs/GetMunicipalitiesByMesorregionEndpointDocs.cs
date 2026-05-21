namespace MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Docs;

public static class GetMunicipalitiesByMesorregionEndpointDocs
{
    public const string Summary = "Lista municípios por mesorregião";

    public const string Description = """
    Retorna municípios vinculados a uma mesorregião do IBGE.

    A paginação é opcional. Sem `page` e `pageSize`, todos os municípios da mesorregião são retornados.

    Observação: alguns municípios recentes podem vir da API do IBGE sem microrregião. Esses municípios não aparecem nesta consulta porque não possuem mesorregião associada no payload atual.

    Exemplo:

    ```http
    GET /municipalities/mesorregions/5102?page=1&pageSize=20
    ```

    Exemplo de sucesso:

    ```json
    {
      "success": true,
      "data": {
        "mesorregionId": 5102,
        "items": [
          {
            "id": 5101852,
            "name": "Bom Jesus do Araguaia",
            "UF": "MT",
            "stateId": 51,
            "stateName": "Mato Grosso",
            "regionId": 5,
            "regionAcronym": "CO",
            "regionName": "Centro-Oeste",
            "mesorregionId": 5102,
            "mesorregionName": "Nordeste Mato-grossense",
            "microregionId": 51009,
            "microregionName": "Norte Araguaia",
            "immediateRegionId": 510014,
            "immediateRegionName": "Confresa - Vila Rica",
            "intermediateRegionId": 5104,
            "intermediateRegionName": "Barra do Garças"
          }
        ],
        "page": 1,
        "pageSize": 20,
        "totalItems": 1,
        "totalPages": 1
      },
      "message": null
    }
    ```

    Exemplo de erro 404:

    ```json
    {
      "success": false,
      "message": "No municipalities were found for mesorregion '9999'."
    }
    ```
    """;
}
