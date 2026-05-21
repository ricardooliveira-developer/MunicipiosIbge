using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Clients;
using MunicipiosIbge.Api.Infrastructure.ExternalServices.Ibge.Models;

namespace MunicipiosIbge.Tests.Common.Fakes;

public sealed class FakeIbgeMunicipalityApi(IReadOnlyList<IbgeMunicipalityResponse> municipalities) : IIbgeMunicipalityApi
{
    public int Calls { get; private set; }

    public Task<IReadOnlyList<IbgeMunicipalityResponse>> GetMunicipalitiesAsync(
        CancellationToken cancellationToken = default)
    {
        Calls++;
        return Task.FromResult(municipalities);
    }
}
