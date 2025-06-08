using finrv.ApiService.Application;

namespace finrv.Application.Services.PositionService.Dtos;

public class UsersPositionsResponseDto
{
    public Guid UserId  { get; set; }
    public string Name  { get; set; }
    public string Email   { get; set; }
    public long TotalPositionSize {  get; set; }
    public decimal TotalPositionValue { get; set; }
    public decimal TotalBrokeragePaid { get; set; }
};