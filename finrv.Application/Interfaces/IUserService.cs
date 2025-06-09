using finrv.ApiService.Application;
using finrv.Application.Services.PositionService.Dtos;
using finrv.Application.Services.UserService.Dtos;

namespace finrv.Application.Interfaces;

public interface IUserService
{
    Task<Pagination<object, AllUsersResponseDto>> AllUsersAsync(AllUsersRequestDto request);
    Task<Pagination<object, AssetsAveragePriceResponseDto>> AveragePriceOfAssetsByUserAsync(Guid userId, AssetsAveragePriceRequestDto query);
    Task<Pagination<TotalUserPositionResponseDto, UserTotalPositionsResponseDto>> GetUserPositionsByIdAsync(Guid userId, UserPositionsRequestDto request);
}