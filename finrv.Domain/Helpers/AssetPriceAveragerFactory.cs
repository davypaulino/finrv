using finrv.Domain.Business.AssetPriceAverager;
using finrv.Domain.Business.AssetPriceAverager.Models;
using finrv.Domain.Enums;
using finrv.Domain.Interfaces;

namespace finrv.Domain.Helpers;

public static class AssetPriceAveragerFactory
{
    public static IAssetPriceAverager CreateCalculator(EVariableIncomeProduct type, IEnumerable<Trade> purchases) => type switch
    {
        EVariableIncomeProduct.Acoes or 
        EVariableIncomeProduct.FundosImobiliarios or
        EVariableIncomeProduct.ETFs or 
        EVariableIncomeProduct.BDRs or
        EVariableIncomeProduct.Criptomoedas or
        EVariableIncomeProduct.MercadoAVista or
        EVariableIncomeProduct.OfertasPublicasIniciais or
        EVariableIncomeProduct.OfertasSubsequentes
            => new AssetPriceAverager(purchases),
        _ => throw new ArgumentException($"No asset price averager found for product type: {type}")
    };
}