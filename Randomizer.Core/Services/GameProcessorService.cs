using Randomizer.Common;
using Randomizer.Core.Abstractions.Infrastructure;
using Randomizer.Core.Abstractions.Persistence;
using Randomizer.Core.DTOs;
using Randomizer.Domain.Entities;
using System.Diagnostics;

namespace Randomizer.Core.Services;

public class GameProcessorService : IGameProcessorService
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
        var validationResult = _validator.Validate(gameConfig);

        if (!validationResult.IsValid)
        {
            return Result<GameConfigDto>.ValidationError(validationResult.ValidationErrors);
        }

        var gameConfigEntity = new GameConfigEntity
        {
            CountOfRounds = gameConfig.CountOfRounds,
            Messages = gameConfig.Messages
                .Select(x => new MessageEntity { Content = x.Content })
                .ToList(),
            Participants = gameConfig.Participants
                .Select((x, i) => new ParticipantEntity { NickName = x.NickName, Position = i })
                .ToList()
        };

        var result = await _uow.GameConfigRepository.AddAsync(gameConfigEntity);

        await _uow.SaveChangesAsync();

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
        var gameData = await _uow.GameConfigRepository.GetFullAsync(gameConfigId);

        if (gameData is null)
        {
            return Result<RoundResultDto>.Error(ErrorMessages.GameConfigNotFound, ApiErrorCodes.NotFound);
        }

        var currentRound = gameData.Rounds.SingleOrDefault(x => x.IsCurrent);

        if (currentRound is null)
        {
            return Result<RoundResultDto>.Error(ErrorMessages.RoundNotFound, ApiErrorCodes.NotFound);
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

        var roundResult = new RoundResultEntity
        {
            MessageId = message.Id,
            WhoPerformActionId = whoPerformAction.Id,
            WhoPerformFeedbackId = whoPerformFeedback.Id,
            RoundId = currentRound.Id,
        };

        roundResult = await _uow.RoundResultRepository.AddAsync(roundResult);

        await _uow.SaveChangesAsync();

        return Result<RoundResultDto>.Success(new RoundResultDto
        {
            Id = roundResult.Id,
            LastRoundResult = (currentRound.RoundResults.Count + 1) == gameData.Participants.Count,
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
        var gameData = await _uow.GameConfigRepository.FindAsync(gameConfigId);

        if (gameData is null)
        {
            return Result<RoundDto>.Error(ErrorMessages.GameConfigNotFound, ApiErrorCodes.NotFound);
        }

        var rounds = await _uow.RoundRepository.GetAllByGameConfigId(gameConfigId);
        var currentRound = rounds.SingleOrDefault(x => x.IsCurrent);

        if (currentRound is not null)
        {
            currentRound.IsCurrent = false;
            currentRound.IsCompleted = true;

            await _uow.RoundRepository.UpdateAsync(currentRound);
        }

        var newStartedRound = await _uow.RoundRepository.AddAsync(new RoundEntity
        {
            IsCurrent = true,
            IsCompleted = false,
            GameConfigId = gameConfigId
        });

        await _uow.SaveChangesAsync();

        return Result<RoundDto>.Success(new RoundDto
        {
            Id = newStartedRound.Id,
            IsCompleted = newStartedRound.IsCompleted,
            IsCurrent = newStartedRound.IsCurrent,
            SequenceNumber = newStartedRound.SequenceNumber,
            GameConfigId = newStartedRound.GameConfigId
        });
    }

    public async Task<Result> UpdateRoundResultWithFeedback(UpdateRoundResultDto roundResultDto)
    {
        var roundResult = await _uow.RoundResultRepository.FindAsync(roundResultDto.Id);

        if (roundResult is null)
        {
            return Result.Error(ErrorMessages.RoundResultNotFound, ApiErrorCodes.NotFound);
        }

        var validationResult = _validator.Validate(roundResultDto);

        if (!validationResult.IsValid)
        {
            return Result.ValidationError(validationResult.ValidationErrors);
        }

        await _uow.RoundResultRepository.UpdateAsync(new RoundResultEntity
        {
            Id = roundResultDto.Id,
            Score = roundResultDto.Score,
            Comment = roundResultDto.Comment,
        });

        await _uow.SaveChangesAsync();

        return Result.Success();
    }
}