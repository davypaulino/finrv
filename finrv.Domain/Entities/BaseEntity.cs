namespace finrv.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; protected set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }
}

