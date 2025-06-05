namespace finrv.Domain.Entities;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public string CreatedBy { get; protected set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    protected BaseEntity()
    {
    }
}

