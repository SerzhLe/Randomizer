using Microsoft.Extensions.DependencyInjection;
using Randomizer.Core.Abstractions.Persistence;

namespace Randomizer.Persistence.Dapper;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnector, DbConnector>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
