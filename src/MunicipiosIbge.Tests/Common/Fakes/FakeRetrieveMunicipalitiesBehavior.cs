using MunicipiosIbge.Api.Common.Behaviors;
using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Tests.Common.Fakes;

public sealed class FakeRetrieveMunicipalitiesBehavior(IReadOnlyList<Municipality> municipalities)
    : IRetrieveMunicipalitiesBehavior
{
    public int RetrieveCalls { get; private set; }

    public Task<IReadOnlyList<Municipality>> RetrieveAsync(CancellationToken cancellationToken = default)
    {
        RetrieveCalls++;
        return Task.FromResult(municipalities);
    }
}
