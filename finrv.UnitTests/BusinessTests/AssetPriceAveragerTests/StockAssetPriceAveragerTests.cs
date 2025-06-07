using finrv.Domain.Business.AssetPriceAverager;
using finrv.Domain.Business.AssetPriceAverager.Models;
using finrv.UnitTests.Helpers;

namespace finrv.UnitTests.BusinessTests.AssetPriceAveragerTests;

public class StockAssetPriceAveragerTests
{
    public static IEnumerable<object[]> StockPurchaseData = AssertPriceAveragerRecords.StockPurchaseData;
    
    [Theory]
    [MemberData(nameof(StockPurchaseData))]
    public void Calculate_ShouldReturnCorrectWeightedAverage_ForStockPurchases(
        IEnumerable<Trade> purchases, decimal expectedWeightedAverage, string description)
    {
        var averager = new StockAssetPriceAverager(purchases);
        decimal actualWeightedAverage = averager.Calculate();
        
        Assert.Equal(expectedWeightedAverage, actualWeightedAverage);
        Assert.True(true, description);
    }
}
