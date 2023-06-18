using Randomizer.Common;
using Randomizer.Core.Abstractions.Infrastructure;
using Randomizer.Core.Abstractions.Persistence;
using Randomizer.Core.DTOs;
using Randomizer.Domain.Entities;
using System.Diagnostics;

namespace Randomizer.Core.Services;

public class GameProcessorService
{
    private readonly IUnitOfWork _uow;
    private readonly IRandomService _randomService;
    private readonly ICoreValidator _validator;

    public GameProcessorService(IUnitOfWork uow, IRandomService randomService, ICoreValidator validator)
    {
        _uow = uow;
        _randomService = randomService;
        _validator = validator;
    }

    public async Task<Result<GameConfigDto>> StartGame(CreateGameConfigDto gameConfig)
    {
        var validationResult = _validator.ValidateStartGame(gameConfig);

        if (!validationResult.IsValid)
        {
            return Result<GameConfigDto>.ValidationError(validationResult.ValidationErrors);
        }

        var gameConfigEntity = new GameConfigEntity
        {
            Id = Guid.NewGuid(),
            CountOfRounds = gameConfig.CountOfRounds,
            Messages = gameConfig.Messages
                .Select((x, i) => new MessageEntity { Content = x.Content, Position = i })
                .ToList(),
            Participants = gameConfig.Participants
                .Select((x, i) => new ParticipantEntity { NickName = x.NickName, Position = i })
                .ToList()
        };

        await _uow.GameConfigRepository.AddAsync(gameConfigEntity);

        await _uow.SaveChangesAsync();

        var result = await _uow.GameConfigRepository.GetById(gameConfigEntity.Id);

        return Result<GameConfigDto>.Success(new GameConfigDto
        {
            Id = result.Id,
            DisplayId = result.DisplayId,
            CountOfRounds = result.CountOfRounds,
            Messages = result.Messages
                .Select(x => new MessageDto { Id = x.Id, Content = x.Content, Position = x.Position })
                .ToList(),
            Participants = result.Participants
                .Select(x => new ParticipantDto { Id = x.Id, NickName = x.NickName, Position = x.Position })
                .ToList()
        });
    }

    public async Task<Result<RoundResultDto>> GetRandomData(Guid gameConfigId)
    {
        var gameData = await _uow.GameConfigRepository.GetById(gameConfigId);

        if (gameData is null)
        {
            return Result<RoundResultDto>.Error(ErrorMessages.GameConfigNotFound, ApiErrorCodes.NotFound);
        }

        var currentRound = gameData.Rounds.SingleOrDefault(x => x.IsCurrent);

        if (currentRound is null)
        {
            return Result<RoundResultDto>.Error(ErrorMessages.CurrentRoundNotFound, ApiErrorCodes.NotFound);
        }

        if (currentRound.RoundResults.Count == gameData.Participants.Count)
        {
            return Result<RoundResultDto>.Error(ErrorMessages.UnableToRandomizeData, ApiErrorCodes.BadRequest);
        }

        int whoPerformActionPosition, whoPerformFeedbackPosition, messagePosition;
        bool alreadyPerformedAction, alreadyPerformedFeedback, alreadyAskedMessage;
        var participantsCount = gameData.Participants.Count;
        var messagesCount = gameData.Messages.Count;

        var stopWatch = new Stopwatch();
        var maxProcessTimePerLoop = new TimeSpan(0, 0, 10);

        stopWatch.Start();

        do
        {
            whoPerformActionPosition = _randomService.GetRandomNumber(0, participantsCount);

            alreadyPerformedAction = currentRound.RoundResults
                .FirstOrDefault(x => x.WhoPerformAction?.Position == whoPerformActionPosition) is not null;

            if (stopWatch.Elapsed > maxProcessTimePerLoop)
            {
                return Result<RoundResultDto>.Error(ErrorMessages.UnableToRandomizeData, ApiErrorCodes.RequestTimeout);
            }
        } while (alreadyPerformedAction);

        stopWatch.Restart();

        do
        {
            whoPerformFeedbackPosition = _randomService.GetRandomNumber(0, participantsCount);

            alreadyPerformedFeedback = currentRound.RoundResults
                .FirstOrDefault(x => x.WhoPerformFeedback?.Position == whoPerformFeedbackPosition) is not null;

            if (stopWatch.Elapsed > maxProcessTimePerLoop)
            {
                return Result<RoundResultDto>.Error(ErrorMessages.UnableToRandomizeData, ApiErrorCodes.RequestTimeout);
            }
        } while (alreadyPerformedFeedback || whoPerformFeedbackPosition == whoPerformActionPosition);

        stopWatch.Restart();

        do
        {
            messagePosition = _randomService.GetRandomNumber(0, messagesCount);

            alreadyAskedMessage = gameData.Rounds
                .SelectMany(x => x.RoundResults)
                .Where(x => x.Message?.Position == messagePosition
                    && x.WhoPerformFeedback?.Position == whoPerformFeedbackPosition)
                .Any();

            if (stopWatch.Elapsed > maxProcessTimePerLoop)
            {
                return Result<RoundResultDto>.Error(ErrorMessages.UnableToRandomizeData, ApiErrorCodes.RequestTimeout);
            }
        } while (alreadyAskedMessage);

        var whoPerformAction = gameData.Participants.Single(x => x.Position == whoPerformActionPosition);
        var whoPerformFeedback = gameData.Participants.Single(x => x.Position == whoPerformFeedbackPosition);
        var message = gameData.Messages.Single(x => x.Position == messagePosition);

        currentRound.RoundResults.Add(new RoundResultEntity
        {
            Id = Guid.NewGuid(),
            MessageId = message.Id,
            WhoPerformActionId = whoPerformAction.Id,
            WhoPerformFeedbackId = whoPerformFeedback.Id,
        });

        await _uow.SaveChangesAsync();

        return Result<RoundResultDto>.Success(new RoundResultDto
        {
            Id = currentRound.Id,
            WhoPerformAction = new ParticipantDto
            {
                Id = whoPerformAction.Id,
                NickName = whoPerformAction.NickName,
                Position = whoPerformAction.Position
            },
            WhoPerformFeedback = new ParticipantDto
            {
                Id = whoPerformFeedback.Id,
                NickName = whoPerformFeedback.NickName,
                Position = whoPerformFeedback.Position
            },
            Message = new MessageDto
            {
                Id = message.Id,
                Content = message.Content,
                Position = message.Position
            }
        });
    }

    public async Task<Result<RoundDto>> StartNewRound(Guid gameConfigId)
    {
        var gameData = await _uow.GameConfigRepository.GetById(gameConfigId);

        if (gameData is null)
        {
            return Result<RoundDto>.Error(ErrorMessages.GameConfigNotFound, ApiErrorCodes.NotFound);
        }

        var currentRound = gameData.Rounds.SingleOrDefault(x => x.IsCurrent);

        if (currentRound is not null)
        {
            currentRound.IsCurrent = false;
            currentRound.IsCompleted = true;
        }

        var newStartedRound = new RoundEntity
        {
            Id = Guid.NewGuid(),
            IsCurrent = true,
            IsCompleted = false,
            GameConfigEntityId = gameConfigId
        };

        await _uow.RoundRepository.AddAsync(newStartedRound);

        await _uow.SaveChangesAsync();

        return Result<RoundDto>.Success(new RoundDto
        {
            Id = newStartedRound.Id,
            IsCompleted = newStartedRound.IsCompleted,
            IsCurrent = newStartedRound.IsCurrent,
            GameConfigId = newStartedRound.GameConfigEntityId
        });
    }
}