using Randomizer.Core.Abstractions;
using Randomizer.Core.DTOs;
using Randomizer.Domain.Entities;

namespace Randomizer.Core.Services;

public class GameLifecycleService
{
    private readonly IGameConfigRepository _gameConfigRepository;

    public GameLifecycleService(IGameConfigRepository gameConfigRepository)
    {
        _gameConfigRepository = gameConfigRepository;
    }

    public async Task<GameConfigDto> StartGame(CreateGameConfigDto gameConfig)
    {
        // validation goes here
        // ...
        var validationResult = true;

        if (!validationResult)
        {
            return new GameConfigDto();
        }

        var gameConfigEntity = new GameConfigEntity
        {
            CountOfRounds = gameConfig.CountOfRounds,
            Messages = gameConfig.Messages
                .Select(x => new MessageEntity { Content = x.Content })
                .ToList(),
            Participants = gameConfig.Participants
                .Select(x => new ParticipantEntity { NickName = x.NickName })
                .ToList()
        };

        _gameConfigRepository.Add(gameConfigEntity);

        await _gameConfigRepository.SaveChangesAsync();

        var result = await _gameConfigRepository.GetLastCreated();

        return new GameConfigDto
        {
            Id = result.Id,
            DisplayId = result.DisplayId,
            CountOfRounds = result.CountOfRounds,
            Messages = result.Messages
                .Select(x => new MessageDto { Id = x.Id, Content = x.Content })
                .ToList(),
            Participants = result.Participants
                .Select(x => new ParticipantDto { Id = x.Id, NickName = x.NickName})
                .ToList()
        };
    }
}

