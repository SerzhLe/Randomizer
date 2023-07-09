using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Randomizer.Common;
using Randomizer.Core.Abstractions.Infrastructure;
using Randomizer.Core.DTOs;

namespace Randomizer.Infrastructure.Validation;
internal class FluentCoreValidator : ICoreValidator
{
    private readonly IServiceProvider _serviceProvider;

    public FluentCoreValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ValidationResult Validate<T>(T dto)
    {
        var validator = _serviceProvider.GetRequiredService<IValidator<T>>();

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