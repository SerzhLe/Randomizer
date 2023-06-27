using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Randomizer.Core.Abstractions.Persistence;

namespace Randomizer.Persistence.Dapper; 
public static class DependencyInjection 
{ 
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnector, DbConnector>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
