using finrv.Shared;

namespace finrv.ApiService.Extensions;

public static class DependencyServicesInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<RequestInfo>();

        return services;
    }
}
