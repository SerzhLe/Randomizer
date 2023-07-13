using FluentValidation;
using Randomizer.Common;

namespace Randomizer.Application.Validation;

public interface ICoreValidator
{
    ValidationResult Validate<T, TDto>(TDto dto) where T : AbstractValidator<TDto>, new();
}