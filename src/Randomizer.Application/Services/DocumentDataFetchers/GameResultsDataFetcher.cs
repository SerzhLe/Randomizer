using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Common;
using Randomizer.Application.Abstractions.Persistence;
using Randomizer.Application.DTOs;
using AutoMapper;
using Randomizer.Domain.Entities;

namespace Randomizer.Application.Services.DocumentDataFetchers;

public class GameResultsDataFetcher : IDocumentDataFetcher<GameResultsDocumentDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GameResultsDataFetcher(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper=mapper;
    }

    public async Task<Result<GameResultsDocumentDto>> FetchDataForDocument(Guid entityId)
    {
        var gameConfig = await _uow.GameConfigRepository.GetFullAsync(entityId);

        if (gameConfig is null)
        {
            return Result<GameResultsDocumentDto>.Error(ErrorMessages.GameConfigNotFound, ApiErrorCodes.NotFound);
        }

        if (gameConfig.Rounds.Count != gameConfig.CountOfRounds)
        {
            return Result<GameResultsDocumentDto>.Error(ErrorMessages.UnableToFinishGame, ApiErrorCodes.BadRequest);
        }

        var gameResults = _mapper.Map<GameConfigEntity, GameResultsDocumentDto>(gameConfig);

        gameResults.Winners = Utils.DefineWinners(gameConfig.Rounds, gameConfig.Participants);

        return Result<GameResultsDocumentDto>.Success(gameResults);
    }
}
