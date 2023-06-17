using Microsoft.Extensions.DependencyInjection;
using Randomizer.Core.Abstractions.Infrastructure;

namespace Randomizer.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IRandomService, RandomService>();

        return services;
    }
}
