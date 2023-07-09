using Randomizer.Common;
using Randomizer.Core.Abstractions.Infrastructure;
using Randomizer.Core.DTOs;

namespace Randomizer.Infrastructure.Validation;
public class FluentCoreValidator : ICoreValidator
{
    public ValidationResult ValidateStartGame(CreateGameConfigDto gameConfig)
    {
        var validator = new StartGameValidator();

        var result = validator.Validate(gameConfig);

        return new ValidationResult
        {
            IsValid = result.IsValid,
            ValidationErrors = result.Errors.Select(x => new ValidationError
            {
                ErrorMessage = x.ErrorMessage,
                PropertyName = x.PropertyName,
                CustomState = x.CustomState
            }).ToList()
        };
    }

    public ValidationResult ValidateUpdateRoundResult(UpdateRoundResultDto roundResult)
    {
        var validator = new UpdateRoundResultValidator();

        var result = validator.Validate(roundResult);

        return new ValidationResult
        {
            IsValid = result.IsValid,
            ValidationErrors = result.Errors.Select(x => new ValidationError
            {
                ErrorMessage = x.ErrorMessage,
                PropertyName = x.PropertyName,
                CustomState = x.CustomState
            }).ToList()
        };
    }
}