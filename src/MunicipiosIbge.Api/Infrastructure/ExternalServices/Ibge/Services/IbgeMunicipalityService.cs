using System.Net;
using System.Text.Json;
using MunicipiosIbge.Api.Common.Exceptions;
using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Clients;
using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Models;

namespace MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Services;

public sealed class IbgeMunicipalityService(
    HttpClient httpClient,
    ILogger<IbgeMunicipalityService> logger) : IIbgeMunicipalityApi
{
    private const string MunicipalitiesEndpoint = "api/v1/localidades/municipios";

    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<IReadOnlyList<IbgeMunicipalityResponse>> GetMunicipalitiesAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Chamando API do IBGE para buscar municípios. Endpoint: {Endpoint}.", MunicipalitiesEndpoint);

            using var response = await httpClient.GetAsync(MunicipalitiesEndpoint, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalServiceException(
                    $"IBGE service returned {(int)response.StatusCode} ({response.StatusCode}).");
            }

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var municipalities = await JsonSerializer.DeserializeAsync<List<IbgeMunicipalityResponse>>(
                stream,
                SerializerOptions,
                cancellationToken);

            logger.LogInformation(
                "API do IBGE respondeu com {MunicipalityCount} municípios.",
                municipalities?.Count ?? 0);

            return municipalities ?? [];
        }
        catch (TaskCanceledException exception) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogError(exception, "Timeout while requesting municipalities from IBGE.");
            throw new ExternalServiceException("Timeout while requesting municipalities from IBGE.", exception);
        }
        catch (HttpRequestException exception) when (exception.StatusCode is not HttpStatusCode.BadGateway)
        {
            logger.LogError(exception, "Error while requesting municipalities from IBGE.");
            throw new ExternalServiceException("Error while requesting municipalities from IBGE.", exception);
        }
        catch (JsonException exception)
        {
            logger.LogError(exception, "Invalid municipality payload returned by IBGE.");
            throw new ExternalServiceException("Invalid municipality payload returned by IBGE.", exception);
        }
    }
}
