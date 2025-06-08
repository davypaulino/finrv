using finrv.ApiService.Application;
using finrv.Application.Interfaces;
using finrv.Application.Services.PositionService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace finrv.ApiService.Routes;

public static class PositionRoutes
{
    public static RouteGroupBuilder MapPositions(this RouteGroupBuilder builder)
    {
        builder.MapGet("/clients-details", async (
            [FromServices] IPositionService service,
            [AsParameters] UsersPositionsRequestDto request) =>
                await service.GetClientsPositionsAsync(request))
            .WithName("Recuperar Posições de clientes")
            .WithDescription("- Recuperar clientes por valores nas posições (ASC | DESC)"
                             + "\n- Recuperar clientes por numéro de posições (ASC | DESC)"
                             + "\n- Recuperar clientes por valor de corretagem (ASC | DESC)")
            .WithDisplayName("Recuperar Posições de clientes")
            .WithTags("positions", "transactions", "users")
            .Produces(StatusCodes.Status200OK,
                typeof(Pagination<TotalPositionsDetailsResponseDto, UsersPositionsResponseDto>))
            .Produces(StatusCodes.Status400BadRequest, typeof(string))
            .Produces(StatusCodes.Status500InternalServerError, typeof(string));
        
        return builder;
    }
}