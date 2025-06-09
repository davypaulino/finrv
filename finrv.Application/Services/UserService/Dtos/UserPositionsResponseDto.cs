namespace finrv.Application.Services.UserService.Dtos;

public class UserTotalPositionsResponseDto
{
    public string Ticker  { get; set; }
    public string Name  { get; set; }
    public uint PositionSize  { get; set; }
    public decimal PositionCostValue { get; set; } 
    public decimal MarketValue { get; set; }
    public decimal AverageCostValue { get; set; }
    public decimal ProfitAndLoss { get; set; }
    public DateTime LastUpdate { get; set; }
}
