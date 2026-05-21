using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using MunicipiosIbge.Api.Common.Exceptions;
using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Services;
using MunicipiosIbge.Tests.Common.Fakes;

namespace MunicipiosIbge.Tests.Infrastructure.ExternalServices;

public sealed class IbgeMunicipalityServiceTests
{
    [Fact]
    public async Task GetMunicipalitiesAsync_WhenApiReturnsSuccess_DeserializesMunicipalities()
    {
        const string json = """
        [
          {
            "id": 5101837,
            "nome": "Boa Esperança do Norte",
            "microrregiao": null,
            "regiao-imediata": {
              "id": 510008,
              "nome": "Sorriso",
              "regiao-intermediaria": {
                "id": 5103,
                "nome": "Sinop",
                "UF": {
                  "id": 51,
                  "sigla": "MT",
                  "nome": "Mato Grosso",
                  "regiao": {
                    "id": 5,
                    "sigla": "CO",
                    "nome": "Centro-Oeste"
                  }
                }
              }
            }
          }
        ]
        """;
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, json);
        var service = CreateService(handler);

        var result = await service.GetMunicipalitiesAsync();

        var municipality = Assert.Single(result);
        Assert.Equal(5101837, municipality.Id);
        Assert.Equal("Boa Esperança do Norte", municipality.Name);
        Assert.Null(municipality.Microregion);
        Assert.Equal("MT", municipality.ImmediateRegion.IntermediateRegion.State.Acronym);
        Assert.Equal("/api/v1/localidades/municipios", handler.LastRequest?.RequestUri?.AbsolutePath);
    }

    [Fact]
    public async Task GetMunicipalitiesAsync_WhenApiReturnsError_ThrowsExternalServiceException()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.InternalServerError, "{}");
        var service = CreateService(handler);

        await Assert.ThrowsAsync<ExternalServiceException>(() => service.GetMunicipalitiesAsync());
    }

    [Fact]
    public async Task GetMunicipalitiesAsync_WhenJsonIsInvalid_ThrowsExternalServiceException()
    {
        var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{ invalid");
        var service = CreateService(handler);

        await Assert.ThrowsAsync<ExternalServiceException>(() => service.GetMunicipalitiesAsync());
    }

    private static IbgeMunicipalityService CreateService(FakeHttpMessageHandler handler)
    {
        return new IbgeMunicipalityService(
            new HttpClient(handler)
            {
                BaseAddress = new Uri("https://servicodados.ibge.gov.br/")
            },
            NullLogger<IbgeMunicipalityService>.Instance);
    }
}
