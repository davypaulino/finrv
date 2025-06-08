namespace finrv.Application.Services.UserService.Dtos;

public record TotalUserPositionResponseDto(
    decimal TotalBrokerageFees,
    long TotalPositionSize,
    decimal TotalPositionCost,
    decimal TotalAveragePrice,
    decimal TotalProfitAndLoss,
    decimal TotalMarketValue);