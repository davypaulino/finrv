using finrv.Domain.Enums;

namespace finrv.Domain.Business.AssetPriceAverager.Models;

public record TradeData(
    long PositionSize,
    ETransactionType Type,
    decimal Brokerage,
    decimal Price,
    DateTime Time);