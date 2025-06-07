using finrv.Domain.Business.AssetPriceAverager.Models;

namespace finrv.Domain.Business.AssetPriceAverager;

public class StockAssetPriceAverager(IEnumerable<Trade> purchases) : AssetPriceAverager(purchases);