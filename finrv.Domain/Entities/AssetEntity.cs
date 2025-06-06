namespace finrv.Domain.Entities;
public class AssetEntity : BaseEntity
{
    public ulong Id { get; set; }
    public string Ticker { get; set; }
    public string Name { get; set; }
    public ICollection<QuotationEntity> Quotations { get; set; } = new List<QuotationEntity>();
    public ICollection<TransactionEntity> Transactions { get; set; } = new List<TransactionEntity>();
    public ICollection<PositionEntity> Positions { get; set; } = new List<PositionEntity>();

    private AssetEntity() { }

    public AssetEntity(string ticker, string name)
    {
        Ticker = ticker;
        Name = name;
    }
}
