using finrv.Application.Interfaces;
using finrv.Application.Services.TransactionService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace finrv.ApiService.Routes;

public static class TransactionRoutes
{
    public static RouteGroupBuilder MapTransactions(this RouteGroupBuilder builder)
    {
        builder.MapGet("/brokerage-fees/earnings", async (
            [FromServices] ITransactionService service) 
                => await service.GetBrokerageFeesEarningsAsync())
            .WithName("Ganhos com Corretagem")
            .WithDescription("Total de ganhos com corretagem.")
            .WithDisplayName("Ganhos com corretagem")
            .WithTags("transactions", "corretora")
            .Produces(StatusCodes.Status200OK, typeof(BrokerageFeesEarningsResponseDto))
            .Produces(StatusCodes.Status500InternalServerError, typeof(string));
        
        return builder;
    }
}