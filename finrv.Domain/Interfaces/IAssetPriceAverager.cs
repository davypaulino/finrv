using finrv.Domain.Business.AssetPriceAverager.Models;

namespace finrv.Domain.Interfaces;

public interface IAssetPriceAverager
{
    decimal Calculate(IEnumerable<TradeData> trades);
}