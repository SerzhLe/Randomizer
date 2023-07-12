using FluentValidation;
using Randomizer.Core.DTOs;

namespace Randomizer.Infrastructure.Validation;
internal class UpdateRoundResultValidator : AbstractValidator<UpdateRoundResultDto>
{
    public UpdateRoundResultValidator()
    {
        RuleFor(x => x.Score).NotEmpty().InclusiveBetween(1.0, 5.0);

        RuleFor(x => x.Comment).MaximumLength(200);
    }
}
