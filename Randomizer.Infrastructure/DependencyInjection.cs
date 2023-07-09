using Microsoft.Extensions.DependencyInjection;
using Randomizer.Core.Abstractions.Infrastructure;
using Randomizer.Infrastructure.Validation;

namespace Randomizer.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IRandomService, RandomService>();
        services.AddScoped<ICoreValidator, FluentCoreValidator>();

        return services;
    }
}
