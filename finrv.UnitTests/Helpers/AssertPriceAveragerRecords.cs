using finrv.Domain.Business.AssetPriceAverager.Models;
using finrv.Domain.Enums;

namespace finrv.UnitTests.Helpers;

public record PurchaseTestCase(
    List<TradeData> Trades,
    decimal ExpectedWeightedAverage,
    string Description);

public static class AssertPriceAveragerRecords
{
    public static IEnumerable<object[]> StockPurchaseAndSaleData =>
        new List<PurchaseTestCase>
        {
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 100, Price: 10.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now),
                    new TradeData(PositionSize: 50, Price: 12.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddHours(-1)),
                    new TradeData(PositionSize: 200, Price: 11.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddHours(-2))
                },
                ExpectedWeightedAverage: 10.86m,
                Description: "1 - Múltiplas compras com valores variados"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 10, Price: 25.50m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now)
                },
                ExpectedWeightedAverage: 25.50m,
                Description: "2 - Apenas uma compra"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>(),
                ExpectedWeightedAverage: 0m,
                Description: "3 - Lista de operações vazia"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 50, Price: 10.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now),
                    new TradeData(PositionSize: 0, Price: 20.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddHours(-1)),
                    new TradeData(PositionSize: 100, Price: 15.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddHours(-2))
                },
                ExpectedWeightedAverage: 13.33m,
                Description: "4 - Compras com quantidade zero (devem ser ignoradas)"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 75, Price: 12.345m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now),
                    new TradeData(PositionSize: 125, Price: 13.567m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now)
                },
                ExpectedWeightedAverage: 13.11m,
                Description: "5 - Compras com preços e quantidades decimais complexos"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 100, Price: 10.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now),
                    new TradeData(PositionSize: 75, Price: -5.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddHours(-2)),
                    new TradeData(PositionSize: 25, Price: 12.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddHours(-3))
                },
                ExpectedWeightedAverage: 10.40m,
                Description: "6 - Compras com preços negativos (devem ser ignoradas na média)"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 10, Price: 1.00m, Type: ETransactionType.Buy, Brokerage: 0.00m, Time: DateTime.Now.AddDays(-5)),
                    new TradeData(PositionSize: 5, Price: 3.00m, Type: ETransactionType.Sell, Brokerage: 0.00m, Time: DateTime.Now.AddDays(-3)),
                    new TradeData(PositionSize: 5, Price: 2.00m, Type: ETransactionType.Buy, Brokerage: 0.00m, Time: DateTime.Now.AddDays(-1))
                },
                ExpectedWeightedAverage: 2.00m,
                Description: "7 - Simples Compra, Venda parcial e nova Compra (PM só recalcula nas compras)"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 100, Price: 10.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddDays(-5)),
                    new TradeData(PositionSize: 100, Price: 15.00m, Type: ETransactionType.Sell, Brokerage: 0m, Time: DateTime.Now.AddDays(-3))
                },
                ExpectedWeightedAverage: 0m,
                Description: "8 - Compra e Venda total da posição (PM final é zero)"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 100, Price: 10.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddDays(-5)),
                    new TradeData(PositionSize: 150, Price: 15.00m, Type: ETransactionType.Sell, Brokerage: 0m, Time: DateTime.Now.AddDays(-3))
                },
                ExpectedWeightedAverage: 0m,
                Description: "9 - Venda maior que a quantidade comprada (deve resultar em posição zero)"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 50, Price: 10.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddDays(-5)),
                    new TradeData(PositionSize: 50, Price: 12.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddDays(-4)),
                    new TradeData(PositionSize: 100, Price: 15.00m, Type: ETransactionType.Sell, Brokerage: 0m, Time: DateTime.Now.AddDays(-3))
                },
                ExpectedWeightedAverage: 0m,
                Description: "10 - Múltiplas compras e venda total da posição (PM final é zero)"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 50, Price: 10.00m, Type: ETransactionType.Sell, Brokerage: 0m, Time: DateTime.Now.AddDays(-5)),
                    new TradeData(PositionSize: 50, Price: 12.00m, Type: ETransactionType.Sell, Brokerage: 0m, Time: DateTime.Now.AddDays(-4))
                },
                ExpectedWeightedAverage: 0m,
                Description: "11 - Apenas vendas sem compra prévia (deve resultar em posição zero)"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 100, Price: 12.00m, Type: ETransactionType.Buy, Brokerage: 4.00m, Time: new DateTime(2025, 5, 20, 11, 0, 0)),
                    new TradeData(PositionSize: 200, Price: 10.00m, Type: ETransactionType.Buy, Brokerage: 5.00m, Time: new DateTime(2025, 5, 15, 9, 30, 0)),
                    new TradeData(PositionSize: 150, Price: 13.00m, Type: ETransactionType.Sell, Brokerage: 3.00m, Time: new DateTime(2025, 5, 22, 14, 0, 0)),
                    new TradeData(PositionSize: 50, Price: 14.00m, Type: ETransactionType.Buy, Brokerage: 2.00m, Time: new DateTime(2025, 5, 25, 10, 15, 0)),
                    new TradeData(PositionSize: 50, Price: 16.00m, Type: ETransactionType.Sell, Brokerage: 2.50m, Time: new DateTime(2025, 5, 28, 16, 45, 0))
                },
                ExpectedWeightedAverage: 11.53m,
                Description: "12 - Cenário complexo: Múltiplas compras e vendas fora de ordem cronológica, com corretagem."
            ),
            
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 10, Price: 1.00m, Type: ETransactionType.Buy, Brokerage: 0.00m, Time: DateTime.Now.AddDays(-5)),
                    new TradeData(PositionSize: 5, Price: 1.50m, Type: ETransactionType.Sell, Brokerage: 0.00m, Time: DateTime.Now.AddDays(-3)),
                    new TradeData(PositionSize: 2, Price: 1.20m, Type: ETransactionType.Buy, Brokerage: 0.00m, Time: DateTime.Now.AddDays(-1))
                },
                ExpectedWeightedAverage: 1.57m,
                Description: "13 - Compra, Venda parcial e nova Compra (PM só recalcula nas compras)"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 50, Price: 15.00m, Type: ETransactionType.Sell, Brokerage: 0m, Time: DateTime.Now.AddDays(-3)),
                    new TradeData(PositionSize: 20, Price: 12.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddDays(-1)),
                    new TradeData(PositionSize: 100, Price: 10.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddDays(-5))
                },
                ExpectedWeightedAverage: 10.57m,
                Description: "14 - Ordem dos trades na lista não cronológica (a lógica deve ordenar por Time)"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 100, Price: 10.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddDays(-5)),
                    new TradeData(PositionSize: 50, Price: 15.00m, Type: ETransactionType.Sell, Brokerage: 0m, Time: DateTime.Now.AddDays(-3)),
                    new TradeData(PositionSize: 20, Price: 12.00m, Type: ETransactionType.Buy, Brokerage: 0m, Time: DateTime.Now.AddDays(-1))
                },
                ExpectedWeightedAverage: 10.57m,
                Description: "15 - Compra, Venda parcial e nova Compra (PM recalcula com novas compras)"
            ),
            new PurchaseTestCase(
                Trades: new List<TradeData>
                {
                    new TradeData(PositionSize: 100, Price: 10.00m, Type: ETransactionType.Buy, Brokerage: 5.00m, Time: DateTime.Now.AddDays(-5)),
                    new TradeData(PositionSize: 50, Price: 12.00m, Type: ETransactionType.Buy, Brokerage: 3.00m, Time: DateTime.Now.AddDays(-4)),
                    new TradeData(PositionSize: 20, Price: 15.00m, Type: ETransactionType.Sell, Brokerage: 1.00m, Time: DateTime.Now.AddDays(-3))
                },
                ExpectedWeightedAverage: 10.72m,
                Description: "16 - Compras e Vendas com corretagem"
            ),
        }
        .Select(tc => new object[] { tc.Trades, tc.ExpectedWeightedAverage, tc.Description });
}