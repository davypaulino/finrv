namespace finrv.Domain.Entities;

public class QuotationEntity : BaseEntity
{
    public ulong Id { get; set; }
    public ulong AssetId { get; set; }
    public AssetEntity Asset { get; set; }
    public decimal Price { get; set; }
    public long Increment { get; set; } = 1;

    private QuotationEntity() { }

    public QuotationEntity(AssetEntity asset, decimal price, DateTime timeStamp)
    {
        Asset = asset;
        AssetId = asset.Id;
        Price = price;
        CreatedAt = timeStamp;
    }

    public void UpdateQuotation(DateTime timeStamp)
    {
        UpdatedAt = timeStamp;
        Increment++;
    }
}

