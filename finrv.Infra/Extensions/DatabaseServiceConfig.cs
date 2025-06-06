using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace finrv.Infra.Extensions;

public static class DatabaseServiceConfig
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<InvestimentDbContext>(options =>
            options.UseMySQL(connectionString!, b => b.MigrationsAssembly("finrv.Infra")));

        return services;
    }
}

