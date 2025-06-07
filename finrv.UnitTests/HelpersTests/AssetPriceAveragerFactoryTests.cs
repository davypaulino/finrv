using finrv.Domain.Business.AssetPriceAverager;
using finrv.Domain.Business.AssetPriceAverager.Models;
using finrv.Domain.Enums;
using finrv.Domain.Helpers;
using finrv.Domain.Interfaces;

namespace finrv.UnitTests.HelpersTests;

public class AssetPriceAveragerFactoryTests
{
    private readonly IEnumerable<Trade> _dummyTrades = new List<Trade>();

    [Theory]
    [InlineData(EVariableIncomeProduct.Acoes, typeof(StockAssetPriceAverager))]
    public void CreateCalculator_ShouldReturnCorrectType_ForGivenProduct(
        EVariableIncomeProduct productType, Type expectedCalculatorType)
    {
        // Arrange && Act
        IAssetPriceAverager calculator = AssetPriceAveragerFactory.CreateCalculator(productType, _dummyTrades);
        
        // Assert
        Assert.NotNull(calculator);
        Assert.IsType(expectedCalculatorType, calculator);
    }
    
    [Fact]
    public void CreateCalculator_ShouldThrowArgumentException_ForUnhandledProductType()
    {
        var unhandledType = (EVariableIncomeProduct)0;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            AssetPriceAveragerFactory.CreateCalculator(unhandledType, _dummyTrades));
        Assert.Contains($"No asset price averager found for product type: {unhandledType}", ex.Message);
    }
}