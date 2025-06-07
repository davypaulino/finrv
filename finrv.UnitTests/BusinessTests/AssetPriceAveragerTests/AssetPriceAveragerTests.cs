using finrv.Domain.Business.AssetPriceAverager;
using finrv.Domain.Business.AssetPriceAverager.Models;
using finrv.UnitTests.Helpers;

namespace finrv.UnitTests.BusinessTests.AssetPriceAveragerTests;

public class AssetPriceAveragerTests
{
    public static IEnumerable<object[]> StockPurchaseData = AssertPriceAveragerRecords.StockPurchaseData;
    
    [Theory]
    [MemberData(nameof(StockPurchaseData))]
    public void Calculate_ShouldReturnCorrectWeightedAverage_ForGeneralAssetPurchases(
        IEnumerable<Trade> purchases, decimal expectedWeightedAverage, string description)
    {
        var averager = new AssetPriceAverager(purchases);
        decimal actualWeightedAverage = averager.Calculate();
        
        Assert.Equal(expectedWeightedAverage, actualWeightedAverage);
        Assert.True(true, description);
    }
}
