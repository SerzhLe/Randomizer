using Randomizer.Common;
using Randomizer.Core.Abstractions;
using Randomizer.Core.Abstractions.Infrastructure;
using Randomizer.Core.Abstractions.Persistence;
using Randomizer.Core.DTOs;
using Randomizer.Domain.Entities;

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
        // get full with all related objects
        var gameData = await _uow.GameConfigRepository.GetById(gameConfigId);

        // move to validation part
        if (gameData is null)
        {
            return Result<RoundResultDto>.ServerError("", 0);
        }

        var currentRound = gameData.Rounds.SingleOrDefault(x => x.IsCurrent);

        // move to validation part
        if (currentRound is null)
        {
            return Result<RoundResultDto>.ServerError("", 0);
        }

        int whoPerformActionPosition, whoPerformFeedbackPosition, messagePosition;
        bool alreadyPerformedAction, alreadyPerformedFeedback, alreadyAskedMessage;
        var participantsCount = gameData.Participants.Count;
        var messagesCount = gameData.Messages.Count;

        // add check for count of messages, rounds and participants
        // otherwise you app may end up with infinite loop
        do
        {
            whoPerformActionPosition = _randomService.GetRandomNumber(0, participantsCount);

            alreadyPerformedAction = currentRound.RoundResults
                .FirstOrDefault(x => x.WhoPerformAction?.Position == whoPerformActionPosition) is not null;
        } while (alreadyPerformedAction);

        do
        {
            whoPerformFeedbackPosition = _randomService.GetRandomNumber(0, participantsCount);

            alreadyPerformedFeedback = currentRound.RoundResults
                .FirstOrDefault(x => x.WhoPerformFeedback?.Position == whoPerformFeedbackPosition) is not null;


        } while (alreadyPerformedFeedback || whoPerformFeedbackPosition == whoPerformActionPosition);

        do
        {
            messagePosition = _randomService.GetRandomNumber(0, messagesCount);

            alreadyAskedMessage = gameData.Rounds
                .SelectMany(x => x.RoundResults)
                .Where(x => x.Message?.Position == messagePosition
                    && x.WhoPerformFeedback?.Position == whoPerformFeedbackPosition)
                .Any();
        } while (alreadyAskedMessage);

        var whoPerformAction = gameData.Participants.SingleOrDefault(x => x.Position == whoPerformActionPosition);
        var whoPerformFeedback = gameData.Participants.SingleOrDefault(x => x.Position == whoPerformFeedbackPosition);
        var message = gameData.Messages.SingleOrDefault(x => x.Position == messagePosition);

        currentRound.RoundResults.Add(new RoundResultEntity
        {
            MessageId = message.Id,
            WhoPerformActionId = whoPerformAction.Id,
            WhoPerformFeedbackId = whoPerformFeedback.Id,
        });

        await _uow.SaveChangesAsync();

        // add logic for fetching Id
        return Result<RoundResultDto>.Success(new RoundResultDto
        {
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
}

