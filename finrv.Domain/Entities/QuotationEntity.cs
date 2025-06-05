using System.Numerics;

namespace finrv.Domain.Entities;

public class QuotationEntity : BaseEntity
{
    public Guid Id { get; set; }
    public long AssetId { get; set; }
    public AssetEntity Asset { get; set; }
    public double Price { get; set; }

    private QuotationEntity() { }

    public QuotationEntity(AssetEntity asset, double price)
    {
        Asset = asset;
        AssetId = asset.Id;
        Price = price;
    }
}

