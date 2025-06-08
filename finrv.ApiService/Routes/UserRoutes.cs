using finrv.ApiService.Application;
using finrv.Application.Interfaces;
using finrv.Application.Services.PositionService.Dtos;
using finrv.Application.Services.UserService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace finrv.ApiService.Routes;

public static class UserRoutes
{
    public static RouteGroupBuilder MapUsers(this RouteGroupBuilder builder)
    {
        builder.MapGet("", async (
            [FromServices] IUserService userService,
            [AsParameters] AllUsersRequestDto request) 
                => await userService.AllUsersAsync(request))
            .WithName("Retorna usuários")
            .WithTags("users")
            .WithDescription("Retornas useuários com paginação")
            .WithDisplayName("Retorna usuários")
            .Produces(StatusCodes.Status200OK, typeof(Pagination<string, object>), contentType: "application/json")
            .Produces(StatusCodes.Status500InternalServerError, typeof(string));

        builder.MapGet("/{userId}/assets-average-price", async (
            [AsParameters] AssetsAveragePriceRequestDto request,
            [FromServices] IUserService userService, 
            Guid userId) 
                => await userService.AveragePriceOfAssetsByUserAsync(userId,  request))
            .WithName("Preço Médio de Ativos")
            .WithTags("users", "assets")
            .WithDescription("Recupera o preço médio de ativos de um usuário")
            .WithDisplayName("Preço Médio de Ativos")
            .Produces(StatusCodes.Status200OK,  typeof(Pagination<AssetsAveragePriceResponseDto, object>), contentType: "application/json")
            .Produces(StatusCodes.Status400BadRequest, typeof(List<string>))
            .Produces(StatusCodes.Status500InternalServerError, typeof(string));

        builder.MapGet("/{userId}/positions", async (
            [FromServices] IUserService service,
            [AsParameters] UsersPositionsRequestDto request,
            Guid userId) 
                => await service.GetUserPositionsByIdAsync(userId, request))
            .WithName("Recupera as posições de um cliente")
            .WithDescription("Recupera as posições de um cliente e um resumo da posição global.")
            .WithTags("users", "positons")
            .WithDisplayName("Recupera as posições de um cliente")
            .Produces(StatusCodes.Status200OK, typeof(Pagination<TotalUserPositionResponseDto, UserPositionsResponseDto>), contentType: "application/json")
            .Produces(StatusCodes.Status400BadRequest, typeof(string))
            .Produces(StatusCodes.Status500InternalServerError, typeof(string));
            
        return builder;
    }
}

