using finrv.Application.Interfaces;
using finrv.Application.Services.AssetService.Dtos;
using finrv.Infra;
using finrv.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace finrv.Application.Services.AssetService;

public class AssetService(
    InvestimentDbContext context,
    ILogger<AssetService> logger,
    RequestInfo requestInfo) : IAssetService
{
    public const string CLASS_NAME = nameof(AssetService);

    public async Task<AssetLatestQuotationResponseDto?> LatestQuotation(string ticker)
    {
        var lastQuotation = await context.Quotation
            .Where(q => q.Asset.Ticker == ticker)
            .OrderByDescending(q => q.UpdatedAt ?? q.CreatedAt)
            .Take(1)
            .Select(q =>
                new AssetLatestQuotationResponseDto(
                    q.Asset.Ticker,
                    q.Asset.Name, 
                    q.Asset.Name,
                    q.Price,
                    q.UpdatedAt ?? q.CreatedAt))
            .FirstOrDefaultAsync();
        
        return lastQuotation;
    }
}