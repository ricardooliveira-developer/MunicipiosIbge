using System.Text.Json.Serialization;

namespace MunicipiosIbge.Api.Domain.Entities;

public sealed class State : BaseAuditableEntity
{
    public int Id { get; private set; }
    public string Acronym { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public int RegionId { get; private set; }

    [JsonInclude]
    public Region Region { get; private set; } = null!;

    [JsonIgnore]
    public ICollection<Mesorregion> Mesorregions { get; private set; } = new List<Mesorregion>();

    [JsonIgnore]
    public ICollection<IntermediateRegion> IntermediateRegions { get; private set; } = new List<IntermediateRegion>();

    private State()
    {
    }

    public State(int id, string acronym, string name, int regionId)
    {
        Id = id;
        Acronym = acronym;
        Name = name;
        RegionId = regionId;
    }

    public void Update(string acronym, string name, int regionId)
    {
        Acronym = acronym;
        Name = name;
        RegionId = regionId;
    }
}
