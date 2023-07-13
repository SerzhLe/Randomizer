using Microsoft.Extensions.DependencyInjection;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Randomizer.Application.Abstractions.Infrastructure;
using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Common;
using Randomizer.Infrastructure.FileSystem.Pdf.Content;
using static Randomizer.Infrastructure.FileSystem.Pdf.PdfCommon;

namespace LynxMarvelTor.BL.Services.FileSystem.Pdf;

public class PdfGenerator : IDocumentGenerator
{
    private readonly IServiceProvider _serviceProvider;

    public PdfGenerator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Result<byte[]> GenerateDocument<TDto>(TDto data) where TDto : BaseDocumentDto
    {
        var document = new Document { Info = { Author = "Randomizer" } };

        DefineBasicStyles(document);

        DefineBasicLayout(document);

        var content = _serviceProvider.GetRequiredService<BaseContent<TDto>>();

        content.DefineContent(document, data);

        var renderer = new PdfDocumentRenderer() { Document = document };

        renderer.RenderDocument();

        using var memoryStream = new MemoryStream();

        renderer.Save(memoryStream, false);

        var result = memoryStream.ToArray();

        return Result<byte[]>.Success(result);
    }
}