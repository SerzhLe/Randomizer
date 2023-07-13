using LynxMarvelTor.BL.Services.FileSystem.Pdf;
using Microsoft.Extensions.DependencyInjection;
using PdfSharp.Fonts;
using Randomizer.Application.Abstractions.Infrastructure;
using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Infrastructure.FileSystem.Pdf;
using Randomizer.Infrastructure.FileSystem.Pdf.Content;

namespace Randomizer.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Pdf services
        GlobalFontSettings.FontResolver = new BasicFontResolver();
        services.AddScoped<BaseContent<GameResultsDocumentDto>, GameResultsContent>();
        services.AddScoped<IDocumentGenerator, PdfGenerator>();

        return services;
    }
}
