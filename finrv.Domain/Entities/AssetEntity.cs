using System.Numerics;

namespace finrv.Domain.Entities;
public class AssetEntity : BaseEntity
{
    public long Id { get; set; }
    public string Ticker { get; set; }
    public string Name { get; set; }
    public ICollection<QuotationEntity> Quotations { get; set; } = new List<QuotationEntity>();

    private AssetEntity() { }

    public AssetEntity(string ticker, string name)
    {
        Ticker = ticker;
        Name = name;
    }
}
