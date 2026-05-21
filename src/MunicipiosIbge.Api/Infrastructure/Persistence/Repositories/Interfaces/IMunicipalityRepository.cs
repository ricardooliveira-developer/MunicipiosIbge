using MunicipiosIbge.Api.Domain.Entities;

namespace MunicipiosIbge.Api.Infrastructure.Persistence.Repositories.Interfaces;

public interface IMunicipalityRepository
{
    Task<IReadOnlyList<Municipality>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Municipality?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Municipality>> GetByMesorregionIdAsync(int mesorregionId, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Municipality> municipalities, CancellationToken cancellationToken = default);
    Task ReplaceAllAsync(
        IEnumerable<Region> regions,
        IEnumerable<State> states,
        IEnumerable<Mesorregion> mesorregions,
        IEnumerable<Microregion> microregions,
        IEnumerable<IntermediateRegion> intermediateRegions,
        IEnumerable<ImmediateRegion> immediateRegions,
        IEnumerable<Municipality> municipalities,
        CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
