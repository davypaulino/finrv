using finrv.Application.Interfaces;
using finrv.Application.Services.AssetService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace finrv.ApiService.Routes;

public static class AssetRoutes
{
    public static RouteGroupBuilder MapAssets(this RouteGroupBuilder builder)
    {
        builder.MapGet("/{ticker}/latest", async (
                [FromServices] IAssetService assetService, 
                string ticker) =>
            {
                var result = await assetService.LatestQuotationAsync(ticker);
                return result is not null ? Results.Ok(result) : Results.NotFound("Ativo não Encontrado");
            })
            .WithName("Ultima cotação de um ativo")
            .WithTags("assets")
            .WithDescription("Recupera a ultima cotação de um ativo a partir de seu código.")
            .WithDisplayName("Ultima cotação de um ativo")
            .Produces(StatusCodes.Status200OK, typeof(AssetLatestQuotationResponseDto), contentType: "application/json")
            .Produces(StatusCodes.Status404NotFound, typeof(string))
            .Produces(StatusCodes.Status400BadRequest, typeof(string))
            .Produces(StatusCodes.Status500InternalServerError, typeof(string));
        
        return builder;
    }
}