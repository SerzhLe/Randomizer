using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Randomizer.Core.Abstractions.Infrastructure;
using Randomizer.Core.DTOs;
using Randomizer.Infrastructure.Validation;
using System.Reflection;

namespace Randomizer.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Scoped, includeInternalTypes: true);
        services.AddScoped<IRandomService, RandomService>();
        services.AddScoped<ICoreValidator, FluentCoreValidator>();

        return services;
    }
}
