using Randomizer.Application.Abstractions.Infrastructure;
using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Application.Services.DocumentDataFetchers;
using Randomizer.Common;

namespace Randomizer.Application.Services;

public class DocumentService<TDto> : IDocumentService<TDto> where TDto : BaseDocumentDto
{
    private readonly IDocumentDataFetcher<TDto> _documentDataFetcher;
    private readonly IDocumentGenerator _documentGenerator;

    public DocumentService(IDocumentDataFetcher<TDto> documentDataFetcher, IDocumentGenerator documentGenerator)
    {
        _documentDataFetcher = documentDataFetcher;
        _documentGenerator = documentGenerator;
    }

    public async Task<Result<byte[]>> GenerateDocumentAsync(Guid entityId)
    {
        var dataResult = await _documentDataFetcher.FetchDataForDocument(entityId);

        if (!dataResult.IsSuccessful)
        {
            return Result<byte[]>.Error(dataResult.ErrorMessage!, dataResult.ApiErrorCode!.Value);
        }

        if (dataResult.Payload is null)
        {
            return Result<byte[]>.Error(ErrorMessages.DefaultError, ApiErrorCodes.BadRequest);
        }

        var result = _documentGenerator.GenerateDocument(dataResult.Payload);

        if (!result.IsSuccessful)
        {
            return Result<byte[]>.Error(result.ErrorMessage!, result.ApiErrorCode!.Value);
        }

        return result;
    }
}
