namespace finrv.Domain.Entities; 

public class UserEntity : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public double BrokeragePercent { get; set; }

    private UserEntity() { }

    public UserEntity(string name, string email, double brokeragePercent)
    {
        Name = name;
        Email = email;
        BrokeragePercent = brokeragePercent;
    }
}
