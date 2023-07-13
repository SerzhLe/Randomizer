using Randomizer.Application.DTOs;
using Randomizer.Common;
using Randomizer.Application.Abstractions.Infrastructure;
using Randomizer.Application.Abstractions.Persistence;
using Randomizer.Domain.Entities;
using System.Diagnostics;
using Randomizer.Application.Validation;
using Randomizer.Infrastructure.Validation;
using AutoMapper;

namespace Randomizer.Application.Services;

public class GameProcessorService : IGameProcessorService
{
    private readonly IUnitOfWork _uow;
    private readonly IRandomService _randomService;
    private readonly ICoreValidator _validator;
    private readonly IMapper _mapper;

    public GameProcessorService(
        IUnitOfWork uow,
        IRandomService randomService,
        ICoreValidator validator,
        IMapper mapper)
    {
        _uow = uow;
        _randomService = randomService;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<Result<GameConfigDto>> StartGame(CreateGameConfigDto gameConfig)
    {
        var validationResult = _validator.Validate<CreateGameConfigValidator, CreateGameConfigDto>(gameConfig);

        if (!validationResult.IsValid)
        {
            return Result<GameConfigDto>.ValidationError(validationResult.ValidationErrors);
        }

        var gameConfigEntity = _mapper.Map<CreateGameConfigDto, GameConfigEntity>(gameConfig);

        var entity = await _uow.GameConfigRepository.AddAsync(gameConfigEntity);

        await _uow.SaveChangesAsync();

        var result = _mapper.Map<GameConfigEntity, GameConfigDto>(entity);

        return Result<GameConfigDto>.Success(result);
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
            SequenceNumber = roundResult.SequenceNumber,
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

        if (gameData.CountOfRounds == rounds.Count)
        {
            return Result<RoundDto>.Error(ErrorMessages.UnableToStartRound, ApiErrorCodes.BadRequest);
        }

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

        var result = _mapper.Map<RoundEntity, RoundDto>(newStartedRound);
        result.LastRound = (rounds.Count + 1) == gameData.CountOfRounds;

        return Result<RoundDto>.Success(result);
    }

    public async Task<Result> UpdateRoundResultWithFeedback(UpdateRoundResultDto roundResultDto)
    {
        var roundResult = await _uow.RoundResultRepository.FindAsync(roundResultDto.Id);

        if (roundResult is null)
        {
            return Result.Error(ErrorMessages.RoundResultNotFound, ApiErrorCodes.NotFound);
        }

        var validationResult = _validator.Validate<UpdateRoundResultValidator, UpdateRoundResultDto>(roundResultDto);

        if (!validationResult.IsValid)
        {
            return Result.ValidationError(validationResult.ValidationErrors);
        }

        var entity = _mapper.Map<UpdateRoundResultDto, RoundResultEntity>(roundResultDto);

        await _uow.RoundResultRepository.UpdateAsync(entity);

        await _uow.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<GameResultsDto>> GetGameResults(Guid gameConfigId)
    {
        var gameData = await _uow.GameConfigRepository.GetFullAsync(gameConfigId);

        if (gameData is null)
        {
            return Result<GameResultsDto>.Error(ErrorMessages.GameConfigNotFound, ApiErrorCodes.NotFound);
        }

        if (gameData.Rounds.Count != gameData.CountOfRounds)
        {
            return Result<GameResultsDto>.Error(ErrorMessages.UnableToFinishGame, ApiErrorCodes.BadRequest);
        }

        var participantsScores = gameData.Rounds
            .SelectMany(x => x.RoundResults.Select(x => new { x.Score, x.WhoPerformActionId }))
            .Where(x => x.Score.HasValue)
            .GroupBy(x => x.WhoPerformActionId)
            .Select(x => new WinnerDto 
            { 
                Id = x.Key, 
                TotalScore = x.Sum(y => y.Score!.Value),
                NickName = gameData.Participants.SingleOrDefault(y => y.Id == x.Key)?.NickName
            })
            .ToList();

        var winners = new List<WinnerDto>();

        if (participantsScores.Any())
        {
            var highestScore = participantsScores.MaxBy(x => x.TotalScore)!.TotalScore;

            winners = participantsScores.Where(x => x.TotalScore == highestScore).ToList();
        }

        var gameResults = new GameResultsDto
        {
            GameId = gameConfigId,
            CountOfRounds = gameData.CountOfRounds,
            Winners = winners
        };

        return Result<GameResultsDto>.Success(gameResults);
    }
}