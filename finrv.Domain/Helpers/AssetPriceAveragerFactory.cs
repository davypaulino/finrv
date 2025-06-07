using finrv.Domain.Business.AssetPriceAverager;
using finrv.Domain.Business.AssetPriceAverager.Models;
using finrv.Domain.Enums;
using finrv.Domain.Interfaces;

namespace finrv.Domain.Helpers;

public static class AssetPriceAveragerFactory
{
    public static IAssetPriceAverager CreateCalculator(EVariableIncomeProduct type, IEnumerable<Trade> purchases) => type switch
    {
        EVariableIncomeProduct.Acoes => new StockAssetPriceAverager(purchases),
        _ => throw new ArgumentException($"No asset price averager found for product type: {type}")
    };
}