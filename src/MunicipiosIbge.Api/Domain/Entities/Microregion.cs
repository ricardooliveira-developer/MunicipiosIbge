using System.Text.Json.Serialization;

namespace MunicipiosIbge.Api.Domain.Entities;

public sealed class Microregion : BaseAuditableEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int MesorregionId { get; private set; }

    [JsonInclude]
    public Mesorregion Mesorregion { get; private set; } = null!;

    [JsonIgnore]
    public ICollection<Municipality> Municipalities { get; private set; } = new List<Municipality>();

    private Microregion()
    {
    }

    public Microregion(int id, string name, int mesorregionId)
    {
        Id = id;
        Name = name;
        MesorregionId = mesorregionId;
    }

    public void Update(string name, int mesorregionId)
    {
        Name = name;
        MesorregionId = mesorregionId;
    }
}
