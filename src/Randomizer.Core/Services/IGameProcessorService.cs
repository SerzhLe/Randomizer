using Randomizer.Common;
using Randomizer.Core.DTOs;

namespace Randomizer.Core.Services;
public interface IGameProcessorService
{
    Task<Result<GameConfigDto>> StartGame(CreateGameConfigDto gameConfig);

    Task<Result<RoundResultDto>> GetRandomData(Guid gameConfigId);

    Task<Result<RoundDto>> StartNewRound(Guid gameConfigId);

    Task<Result> UpdateRoundResultWithFeedback(UpdateRoundResultDto roundResultDto);
}
