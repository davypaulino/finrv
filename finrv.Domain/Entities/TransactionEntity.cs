using finrv.Domain.Enums;

namespace finrv.Domain.Entities;

public class TransactionEntity : BaseEntity
{
    public ulong Id { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public ulong AssetId { get; set; }
    public AssetEntity Asset { get; set; }
    public uint PositionSize { get; set; }
    public decimal UnitPrice { get; set; }
    public ETransactionType Type { get; set; }
    public decimal BrokerageValue { get; set; }

    private TransactionEntity() { }

    public TransactionEntity(
        UserEntity user,
        AssetEntity asset,
        uint positionSize,
        decimal unitPrice,
        ETransactionType type,
        decimal brokerageValue)
    {
        User = user;
        UserId = user.Id;
        Asset = asset;
        AssetId = asset.Id;
        PositionSize = positionSize;
        UnitPrice = unitPrice;
        Type = type;
        BrokerageValue = brokerageValue;
    }
}
