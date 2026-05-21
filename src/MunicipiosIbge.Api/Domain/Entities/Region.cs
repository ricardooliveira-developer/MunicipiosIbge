using System.Text.Json.Serialization;

namespace MunicipiosIbge.Api.Domain.Entities;

public sealed class Region : BaseAuditableEntity
{
    public int Id { get; private set; }
    public string Acronym { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    [JsonIgnore]
    public ICollection<State> States { get; private set; } = new List<State>();

    private Region()
    {
    }

    public Region(int id, string acronym, string name)
    {
        Id = id;
        Acronym = acronym;
        Name = name;
    }

    public void Update(string acronym, string name)
    {
        Acronym = acronym;
        Name = name;
    }
}
