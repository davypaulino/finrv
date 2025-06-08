namespace finrv.Domain.Business.AssetPriceAverager.Models;

public record AverageResult(
    decimal PositionSize,
    decimal Price);