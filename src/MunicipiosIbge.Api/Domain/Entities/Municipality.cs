using System.Text.Json.Serialization;

namespace MunicipiosIbge.Api.Domain.Entities;

public sealed class Municipality : BaseAuditableEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int? MicroregionId { get; private set; }
    public int ImmediateRegionId { get; private set; }

    [JsonInclude]
    public Microregion? Microregion { get; private set; }

    [JsonInclude]
    public ImmediateRegion ImmediateRegion { get; private set; } = null!;

    private Municipality()
    {
    }

    public Municipality(int id, string name, int? microregionId, int immediateRegionId)
    {
        Id = id;
        Name = name;
        MicroregionId = microregionId;
        ImmediateRegionId = immediateRegionId;
    }

    public void Update(string name, int? microregionId, int immediateRegionId)
    {
        Name = name;
        MicroregionId = microregionId;
        ImmediateRegionId = immediateRegionId;
    }
}
