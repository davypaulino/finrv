using finrv.ApiService.Application;
using finrv.Application.Services.PositionService.Dtos;

namespace finrv.Application.Interfaces;

public interface IPositionService
{
    Task<Pagination<TotalPositionsDetailsResponseDto, UsersPositionsResponseDto>>
        GetClientsPositionsAsync(UsersPositionsRequestDto request);
}