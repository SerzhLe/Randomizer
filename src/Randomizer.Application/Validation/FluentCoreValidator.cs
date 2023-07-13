using FluentValidation;
using Randomizer.Common;

namespace Randomizer.Application.Validation;

public class FluentCoreValidator : ICoreValidator
{
    public ValidationResult Validate<T, TDto>(TDto dto) where T : AbstractValidator<TDto>, new()
    {
        var validator = new T();

        var result = validator.Validate(dto);

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