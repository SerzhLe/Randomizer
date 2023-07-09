using Randomizer.Common;
using Randomizer.Core.DTOs;

namespace Randomizer.Core.Abstractions.Infrastructure;
public interface ICoreValidator
{
    ValidationResult ValidateStartGame(CreateGameConfigDto gameConfig);

    ValidationResult ValidateUpdateRoundResult(UpdateRoundResultDto roundResult);
}