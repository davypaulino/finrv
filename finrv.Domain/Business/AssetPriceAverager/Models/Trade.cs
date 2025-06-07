namespace finrv.Domain.Business.AssetPriceAverager.Models;

public record Trade(uint PositionSize, decimal Price, DateTime Time);