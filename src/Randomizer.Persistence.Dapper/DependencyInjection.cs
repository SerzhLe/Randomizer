using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Randomizer.Core.Abstractions.Persistence;
using Randomizer.Dapper;

namespace Randomizer.Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnector, DbConnector>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
