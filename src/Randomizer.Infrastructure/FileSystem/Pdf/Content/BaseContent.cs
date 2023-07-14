using MigraDoc.DocumentObjectModel;
using Randomizer.Application.DTOs.FileSystem;

namespace Randomizer.Infrastructure.FileSystem.Pdf.Content;

public abstract class BaseContent<TDto> where TDto : BaseDocumentDto
{
    protected const string EmptyValue = "—";
    protected const string FullDateTimeFormat = "dd MMM yyyy, HH:mm:ss";

    public abstract void DefineContent(Document document, TDto data);

    protected string ValueOrDefault<T>(T? value) where T : struct => value.HasValue ? value.Value.ToString()! : EmptyValue;

    protected string ValueOrDefault(string value) => !string.IsNullOrWhiteSpace(value) ? value : EmptyValue;
}
