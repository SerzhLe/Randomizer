using MigraDoc.DocumentObjectModel;
using Randomizer.Application.DTOs.FileSystem;

namespace Randomizer.Infrastructure.FileSystem.Pdf.Content;

public class GameResultsContent : BaseContent<GameResultsDocumentDto>
{
    public override void DefineContent(Document document, GameResultsDocumentDto data)
    {
        document.Info.Title = $"Game{data.DisplayId} Results";


    }
}
