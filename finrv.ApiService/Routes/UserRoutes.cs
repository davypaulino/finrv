using finrv.ApiService.Application;
using finrv.Application.Interfaces;
using finrv.Application.Services.UserService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace finrv.ApiService.Routes;

public static class UserRoutes
{
    public static RouteGroupBuilder MapUsers(this RouteGroupBuilder builder)
    {
        builder.MapGet("", async ([FromServices] IUserService userService, [AsParameters] AllUsersRequestDto request) =>
            {
                return await userService.AllUsers(request);
            })
            .WithName("Retorna usuários")
            .WithTags("users")
            .WithDescription("Retornas useuários com paginação")
            .WithDisplayName("Retorna usuários")
            .Produces(StatusCodes.Status200OK, typeof(Pagination<string, object>), contentType: "application/json")
            .Produces(StatusCodes.Status500InternalServerError, typeof(string));
        
        return builder;
    }
}

