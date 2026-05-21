using MunicipiosIbge.Api.Domain.Entities;
using MunicipiosIbge.Api.Infrastructure.Persistence.Repositories.Interfaces;

namespace MunicipiosIbge.Tests.Common.Fakes;

public sealed class FakeMunicipalityRepository(IReadOnlyList<Municipality>? municipalities = null) : IMunicipalityRepository
{
    private IReadOnlyList<Municipality> municipalities = municipalities ?? [];

    public int GetAllCalls { get; private set; }
    public int ReplaceAllCalls { get; private set; }
    public IReadOnlyList<Municipality> LastReplacedMunicipalities { get; private set; } = [];
    public IReadOnlyList<Microregion> LastReplacedMicroregions { get; private set; } = [];

    public Task<IReadOnlyList<Municipality>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        GetAllCalls++;
        return Task.FromResult(municipalities);
    }

    public Task<Municipality?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(municipalities.FirstOrDefault(municipality => municipality.Id == id));
    }

    public Task<IReadOnlyList<Municipality>> GetByMesorregionIdAsync(
        int mesorregionId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Municipality>>(
            municipalities.Where(municipality => municipality.Microregion?.MesorregionId == mesorregionId).ToList());
    }

    public Task AddRangeAsync(IEnumerable<Municipality> municipalities, CancellationToken cancellationToken = default)
    {
        this.municipalities = municipalities.ToList();
        return Task.CompletedTask;
    }

    public Task ReplaceAllAsync(
        IEnumerable<Region> regions,
        IEnumerable<State> states,
        IEnumerable<Mesorregion> mesorregions,
        IEnumerable<Microregion> microregions,
        IEnumerable<IntermediateRegion> intermediateRegions,
        IEnumerable<ImmediateRegion> immediateRegions,
        IEnumerable<Municipality> municipalities,
        CancellationToken cancellationToken = default)
    {
        ReplaceAllCalls++;
        LastReplacedMunicipalities = municipalities.ToList();
        LastReplacedMicroregions = microregions.ToList();
        this.municipalities = LastReplacedMunicipalities;
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
