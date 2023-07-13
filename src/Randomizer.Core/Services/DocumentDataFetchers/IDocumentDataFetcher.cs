using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Common;

namespace Randomizer.Application.Services.DocumentDataFetchers;

public interface IDocumentDataFetcher<TDto> where TDto : BaseDocumentDto
{
    Task<Result<TDto>> FetchDataForDocument(Guid entityId);
}
