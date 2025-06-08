using finrv.Application.Services.AssetService.Dtos;

namespace finrv.Application.Interfaces;

public interface IAssetService
{
    Task<AssetLatestQuotationResponseDto?> LatestQuotationAsync(string ticker);
}