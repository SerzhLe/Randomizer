using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Randomizer.Core.Abstractions.Persistence;

namespace Randomizer.Persistence.Dapper; 
public static class DependencyInjection 
{ 
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>(provider =>
        {
            var connectionString = configuration.GetConnectionString("randomizerDb");

            return new UnitOfWork(new NpgsqlConnection(connectionString));
        });
    }
}
