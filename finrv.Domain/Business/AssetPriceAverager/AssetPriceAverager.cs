using finrv.Domain.Business.AssetPriceAverager.Models;
using finrv.Domain.Enums;
using finrv.Domain.Interfaces;

namespace finrv.Domain.Business.AssetPriceAverager;

public class AssetPriceAverager() : IAssetPriceAverager
{
    private static decimal SimpleCalculate(IEnumerable<TradeData> trades)
    {
        var tradeDatas = trades.ToList();
        if (!tradeDatas.Any())
        {
            return 0m;
        }
        
        var validAndSortedTrades = tradeDatas
            .Where(p => p is { Price: > 0, PositionSize: > 0 })
            .OrderBy(p => p.Time)
            .ToList();

        decimal currentQuantity = 0m;
        decimal currentTotalCost = 0m;
        decimal currentWeightedAverage = 0m;

        foreach (var trade in validAndSortedTrades)
        {
            if (trade.Type == ETransactionType.Buy)
            {
                decimal costOfThisBuy = (trade.Price * ((decimal)trade.PositionSize)) + trade.Brokerage;
                
                currentTotalCost += costOfThisBuy;
                currentQuantity += trade.PositionSize;
                currentWeightedAverage = currentQuantity > 0 ? currentTotalCost / (decimal)currentQuantity : 0m;
            }
            else if (trade.Type == ETransactionType.Sell)
            {
                if (trade.PositionSize >= currentQuantity)
                {
                    currentQuantity = 0m;
                    currentTotalCost = 0m;
                    currentWeightedAverage = 0m;
                    continue;
                }
                decimal costReductionDueToSell = trade.PositionSize * currentWeightedAverage;
                currentTotalCost -= costReductionDueToSell;
                currentQuantity -= trade.PositionSize;
            }
        }
        
        if (currentQuantity <= 0)
        {
            return 0m;
        }
        
        return Math.Round(currentWeightedAverage, 2, MidpointRounding.AwayFromZero);
    }

    public virtual decimal Calculate(IEnumerable<TradeData> trades)
    {
        return SimpleCalculate(trades);
    }
}