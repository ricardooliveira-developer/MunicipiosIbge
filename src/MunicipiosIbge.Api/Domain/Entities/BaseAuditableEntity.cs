namespace MunicipiosIbge.Api.Domain.Entities;

public abstract class BaseAuditableEntity
{
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public void MarkAsCreated(DateTime utcNow)
    {
        CreatedAt = utcNow;
    }

    public void MarkAsUpdated(DateTime utcNow)
    {
        UpdatedAt = utcNow;
    }
}
