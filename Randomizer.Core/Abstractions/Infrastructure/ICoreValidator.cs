using Randomizer.Core.DTOs;

namespace Randomizer.Core.Abstractions.Infrastructure;
public interface ICoreValidator
{
    bool ValidateStartGame(CreateGameConfigDto gameConfig);
}