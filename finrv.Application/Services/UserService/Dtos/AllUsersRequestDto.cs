namespace finrv.Application.Services.UserService.Dtos;

public record AllUsersRequestDto(uint Page = 1, uint PageSize = 10);