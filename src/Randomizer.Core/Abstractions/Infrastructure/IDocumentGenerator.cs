using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Common;

namespace Randomizer.Application.Abstractions.Infrastructure;

public interface IDocumentGenerator
{
    Result<byte[]> GenerateDocument<TDto>(TDto data) where TDto : BaseDocumentDto;
}
