﻿using finrv.Application.Interfaces;
using finrv.Application.Services.AssetService;
using finrv.Application.Services.PositionService;
using finrv.Application.Services.TransactionService;
using finrv.Application.Services.UserService;
using finrv.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace finrv.Application.Extensions;

public static class DependencyServicesInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<RequestInfo>();

        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAssetService, AssetService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IPositionService, PositionService>();

        return services;
    }
}
