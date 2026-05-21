using System.Text.Json.Serialization;

namespace MunicipiosIbge.Api.Domain.Entities;

public sealed class ImmediateRegion : BaseAuditableEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int IntermediateRegionId { get; private set; }

    [JsonInclude]
    public IntermediateRegion IntermediateRegion { get; private set; } = null!;

    [JsonIgnore]
    public ICollection<Municipality> Municipalities { get; private set; } = new List<Municipality>();

    private ImmediateRegion()
    {
    }

    public ImmediateRegion(int id, string name, int intermediateRegionId)
    {
        Id = id;
        Name = name;
        IntermediateRegionId = intermediateRegionId;
    }

    public void Update(string name, int intermediateRegionId)
    {
        Name = name;
        IntermediateRegionId = intermediateRegionId;
    }
}
