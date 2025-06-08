namespace finrv.Application.Services.AssetService.Dtos;

public record AssetLatestQuotationResponseDto(
    string Ticker,
    string Name,
    string Description,
    decimal Price,
    DateTime LastUpdate);