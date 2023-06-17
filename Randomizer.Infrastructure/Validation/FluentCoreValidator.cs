using Randomizer.Core.Abstractions.Infrastructure;
using Randomizer.Core.DTOs;

namespace Randomizer.Infrastructure.Validation;
public class FluentCoreValidator : ICoreValidator
{
    public bool ValidateStartGame(CreateGameConfigDto gameConfig)
    {
        var validator = new StartGameValidator();

        return validator.Validate(gameConfig);
    }
}