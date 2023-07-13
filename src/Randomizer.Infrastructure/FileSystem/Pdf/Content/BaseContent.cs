using MigraDoc.DocumentObjectModel;
using Randomizer.Application.DTOs.FileSystem;
using System.Globalization;

namespace Randomizer.Infrastructure.FileSystem.Pdf.Content;

public abstract class BaseContent<TDto> where TDto : BaseDocumentDto
{
    protected const string EmptyValue = "—";
    protected const string OnlyDateFormat = "dd MMM yyyy";
    protected const string FullDateTimeFormat = "dd MMM yyyy, HH:mm:ss";

    public abstract void DefineContent(Document document, TDto data);

    protected string ValueOrDefault<T>(T? value) where T : struct => value.HasValue ? value.Value.ToString()! : EmptyValue;

    protected string ValueOrDefault<T>(T? value, string outputWhenNotEmpty) where T : struct => value.HasValue ? outputWhenNotEmpty : EmptyValue;

    protected string ValueOrDefault(DateTime? value, string format) => value.HasValue ? value.Value.ToString(format, CultureInfo.InvariantCulture) : EmptyValue;

    protected string ValueOrDefault(string value) => !string.IsNullOrWhiteSpace(value) ? value : EmptyValue;

    protected string ValueOrDefault(string value, string outputWhenNotEmpty) => !string.IsNullOrWhiteSpace(value) ? outputWhenNotEmpty : EmptyValue;
}
