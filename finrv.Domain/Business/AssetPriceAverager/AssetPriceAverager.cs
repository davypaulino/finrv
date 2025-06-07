using finrv.Domain.Business.AssetPriceAverager.Models;
using finrv.Domain.Interfaces;

namespace finrv.Domain.Business.AssetPriceAverager;

public class AssetPriceAverager(IEnumerable<Trade> purchases) : IAssetPriceAverager
{
    private readonly IEnumerable<Trade> _purchases = purchases;

    private decimal SimpleCalculate()
    {
        if (!_purchases.Any())
        {
            return 0m;
        }

        var result = new Trade(
            (uint)_purchases.Sum(p => (long)p.PositionSize),
            _purchases.Sum(p => p.Price * p.PositionSize),
            DateTime.Now
        );
        
        return result.PositionSize > 0 ? Math.Round((result.Price / result.PositionSize), 2, MidpointRounding.AwayFromZero) : 0m;
    }

    public virtual decimal Calculate()
    {
        return SimpleCalculate();
    }
}