using FluentValidation;
using LynxMarvelTor.BL.Services.FileSystem.Pdf;
using Microsoft.Extensions.DependencyInjection;
using PdfSharp.Fonts;
using Randomizer.Application.Abstractions.Infrastructure;
using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Application.Validation;
using Randomizer.Infrastructure.FileSystem.Pdf;
using Randomizer.Infrastructure.FileSystem.Pdf.Content;
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

        // Pdf services
        GlobalFontSettings.FontResolver = new BasicFontResolver();
        services.AddScoped<BaseContent<GameResultsDocumentDto>, GameResultsContent>();
        services.AddScoped<IDocumentGenerator, PdfGenerator>();

        return services;
    }
}
