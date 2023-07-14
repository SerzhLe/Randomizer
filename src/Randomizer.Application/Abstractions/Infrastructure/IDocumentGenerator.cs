using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Common;

namespace Randomizer.Application.Abstractions.Infrastructure;

/// <summary>
/// Service for generating documents.
/// </summary>
public interface IDocumentGenerator
{
    /// <summary>
    /// Generates specific document based on some data.
    /// </summary>
    /// <typeparam name="TDto">Specific data model type.</typeparam>
    /// <param name="data">Data itself.</param>
    /// <returns>Content of a document.</returns>
    Result<byte[]> GenerateDocument<TDto>(TDto data) where TDto : BaseDocumentDto;
}
