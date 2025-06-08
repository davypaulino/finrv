using finrv.Domain.Business.AssetPriceAverager;
using finrv.Domain.Business.AssetPriceAverager.Models;
using finrv.Domain.Interfaces;
using finrv.UnitTests.Helpers;
using Xunit.Abstractions;

namespace finrv.UnitTests.BusinessTests.AssetPriceAveragerTests;

public class AssetPriceAveragerTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AssetPriceAveragerTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public static IEnumerable<object[]> StockPurchaseData = AssertPriceAveragerRecords.StockPurchaseAndSaleData;
    
    [Theory]
    [MemberData(nameof(StockPurchaseData))]
    public void Calculate_ShouldReturnCorrectWeightedAverage_ForGeneralAssetPurchases(
        IEnumerable<TradeData> trades, decimal expectedWeightedAverage, string description)
    {
        IAssetPriceAverager averager = new AssetPriceAverager();
        decimal actualWeightedAverage = averager.Calculate(trades);
        
        _testOutputHelper.WriteLine(description);
        Assert.Equal(expectedWeightedAverage, actualWeightedAverage);
        Assert.True(true, description);
    }
}
