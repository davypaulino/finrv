namespace finrv.Application.Services.UserService.Dtos;

public record UserPositionsResponseDto(
    string Ticker,
    string Name,
    uint PositionSize,
    decimal PositionCostValue,
    decimal MarketValue,
    decimal AverageCostValue,
    decimal ProfitAndLoss,
    DateTime LastUpdate);