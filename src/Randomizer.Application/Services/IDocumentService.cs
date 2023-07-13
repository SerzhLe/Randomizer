using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Common;

namespace Randomizer.Application.Services;

public interface IDocumentService<TDto> where TDto : BaseDocumentDto
{
    Task<Result<byte[]>> GenerateDocumentAsync(Guid entityId);
}
