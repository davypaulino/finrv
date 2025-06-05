namespace finrv.Domain.Entities; 

public class UserEntity : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Email { get; set; }
    public double BrokeragePercent { get; set; }

    public ICollection<TransactionEntity> Transactions { get; set; } = new List<TransactionEntity>();
    public ICollection<PositionEntity> Positions { get; set; } = new List<PositionEntity>();

    private UserEntity() { }

    public UserEntity(string name, string email, double brokeragePercent)
    {
        Name = name;
        Email = email;
        BrokeragePercent = brokeragePercent;
    }
}
