namespace finrv.Application.Services.UserService.Dtos;

public record AssetsAveragePriceResponseDto(
    string Ticker,
    decimal AveragePrice,
    DateTime LastUpdate);