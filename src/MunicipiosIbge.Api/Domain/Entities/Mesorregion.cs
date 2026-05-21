using System.Text.Json.Serialization;

namespace MunicipiosIbge.Api.Domain.Entities;

public sealed class Mesorregion : BaseAuditableEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int StateId { get; private set; }

    [JsonInclude]
    public State State { get; private set; } = null!;

    [JsonIgnore]
    public ICollection<Microregion> Microregions { get; private set; } = new List<Microregion>();

    private Mesorregion()
    {
    }

    public Mesorregion(int id, string name, int stateId)
    {
        Id = id;
        Name = name;
        StateId = stateId;
    }

    public void Update(string name, int stateId)
    {
        Name = name;
        StateId = stateId;
    }
}
