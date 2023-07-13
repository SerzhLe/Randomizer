using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Common;
using Randomizer.Application.Abstractions.Persistence;
using Randomizer.Application.DTOs;

namespace Randomizer.Application.Services.DocumentDataFetchers;

public class GameResultsDataFetcher : IDocumentDataFetcher<GameResultsDocumentDto>
{
    private readonly IUnitOfWork _uow;

    public GameResultsDataFetcher(IUnitOfWork uow)
    {
        _uow = uow;
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

        var gameResults = new GameResultsDocumentDto
        {
            Id = gameConfig.Id,
            DisplayId = gameConfig.DisplayId,
            CountOfRounds = gameConfig.CountOfRounds,
            Messages = gameConfig.Messages.Select(x => new MessageDto
            {
                Id = x.Id,
                Content = x.Content,
                Position = x.Position
            }).ToList(),
            Participants = gameConfig.Participants.Select(x => new ParticipantDto
            {
                Id = x.Id,
                NickName = x.NickName,
                Position = x.Position
            }).ToList(),
            Rounds = gameConfig.Rounds.Select(x => new RoundDocumentDto
            {
                Id = x.Id,
                SequenceNumber = x.SequenceNumber,
                RoundResults = x.RoundResults.Select(y => new RoundResultDto
                {
                    Id = y.Id,
                    Comment = y.Comment,
                    Score = y.Score,
                    Message = new MessageDto { Id = y.Message.Id, Content = y.Message.Content, Position = y.Message.Position },
                    WhoPerformAction = new ParticipantDto { Id = y.WhoPerformAction.Id, NickName = y.WhoPerformAction.NickName, Position = y.WhoPerformAction.Position },
                    WhoPerformFeedback = new ParticipantDto { Id = y.WhoPerformFeedback.Id, NickName = y.WhoPerformFeedback.NickName, Position = y.WhoPerformFeedback.Position }
                }).ToList()
            }).ToList()
        };

        var participantsScores = gameConfig.Rounds
            .SelectMany(x => x.RoundResults.Select(x => new { x.Score, x.WhoPerformActionId }))
            .Where(x => x.Score.HasValue)
            .GroupBy(x => x.WhoPerformActionId)
            .Select(x => new WinnerDto
            {
                Id = x.Key,
                TotalScore = x.Sum(y => y.Score!.Value),
                NickName = gameConfig.Participants.SingleOrDefault(y => y.Id == x.Key)?.NickName
            })
            .ToList();

        var winners = new List<WinnerDto>();

        if (participantsScores.Any())
        {
            var highestScore = participantsScores.MaxBy(x => x.TotalScore)!.TotalScore;

            winners = participantsScores.Where(x => x.TotalScore == highestScore).ToList();
        }

        gameResults.Winners = winners;

        return Result<GameResultsDocumentDto>.Success(gameResults);
    }
}
