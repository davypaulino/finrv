namespace finrv.Domain.Entities;

public class PositionEntity : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public ulong AssetId { get; set; }
    public AssetEntity Asset { get; set; }
    public uint PositionSize { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal ProfitAndLoss { get; set; }

    private PositionEntity() { }

    public PositionEntity(UserEntity user, AssetEntity asset, uint positionSize, decimal averagePrice, decimal profitAndLoss)
    {
        Id = Guid.NewGuid();
        UserId = user.Id;
        User = user;
        AssetId = asset.Id;
        Asset = asset;
        PositionSize = positionSize;
        AveragePrice = averagePrice;
        ProfitAndLoss = profitAndLoss;
    }
}
