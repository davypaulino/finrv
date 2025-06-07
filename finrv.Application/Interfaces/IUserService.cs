using finrv.ApiService.Application;
using finrv.Application.Services.UserService.Dtos;

namespace finrv.Application.Interfaces;

public interface IUserService
{
    Task<Pagination<object, AllUsersResponseDto>> AllUsers(AllUsersRequestDto request);
}