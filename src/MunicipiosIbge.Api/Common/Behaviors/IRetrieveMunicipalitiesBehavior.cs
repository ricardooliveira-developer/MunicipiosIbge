using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Api.Common.Behaviors;

public interface IRetrieveMunicipalitiesBehavior
{
    Task<IReadOnlyList<Municipality>> RetrieveAsync(CancellationToken cancellationToken = default);
}
