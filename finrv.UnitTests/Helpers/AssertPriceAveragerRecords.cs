using finrv.Domain.Business.AssetPriceAverager.Models;

namespace finrv.UnitTests.Helpers;

public record PurchaseTestCase(List<Trade> Trades, decimal ExpectedWeightedAverage, string Description);

public static class AssertPriceAveragerRecords
{
    public static IEnumerable<object[]> StockPurchaseData =>
        new List<PurchaseTestCase>
        {
            new PurchaseTestCase(
                Trades: new List<Trade>
                {
                    new Trade(PositionSize: 100, Price: 10.00m, Time: DateTime.Now),
                    new Trade(PositionSize: 50, Price: 12.00m, Time: DateTime.Now.AddHours(-1)),
                    new Trade(PositionSize: 200, Price: 11.00m, Time: DateTime.Now.AddHours(-2))
                },
                ExpectedWeightedAverage: 10.86m,
                Description: "Múltiplas compras com valores variados"
            ),
            new PurchaseTestCase(
                Trades: new List<Trade>
                {
                    new Trade(PositionSize: 10, Price: 25.50m, Time: DateTime.Now)
                },
                ExpectedWeightedAverage: 25.50m,
                Description: "Apenas uma compra"
            ),
            new PurchaseTestCase(
                Trades: new List<Trade>(),
                ExpectedWeightedAverage: 0m,
                Description: "Lista de compras vazia"
            ),
            new PurchaseTestCase(
                Trades: new List<Trade>
                {
                    new Trade(PositionSize: 50, Price: 10.00m, Time: DateTime.Now),
                    new Trade(PositionSize: 0, Price: 20.00m, Time: DateTime.Now.AddHours(-1)), // Ignorado
                    new Trade(PositionSize: 100, Price: 15.00m, Time: DateTime.Now.AddHours(-2))
                },
                ExpectedWeightedAverage: 13.33m,
                Description: "Compras com quantidade zero (devem ser ignoradas)"
            ),
            new PurchaseTestCase(
                Trades: new List<Trade>
                {
                    new Trade(PositionSize: 75, Price: 12.345m, Time: DateTime.Now),
                    new Trade(PositionSize: 125, Price: 13.567m, Time: DateTime.Now)
                },
                ExpectedWeightedAverage: 13.11m,
                Description: "Compras com preços e quantidades decimais complexos"
            ),
        }
        .Select(tc => new object[] { tc.Trades, tc.ExpectedWeightedAverage, tc.Description });
}