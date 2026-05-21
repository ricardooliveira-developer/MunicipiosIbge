using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Models;

namespace MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Clients;

public interface IIbgeMunicipalityApi
{
    Task<IReadOnlyList<IbgeMunicipalityResponse>> GetMunicipalitiesAsync(CancellationToken cancellationToken = default);
}
