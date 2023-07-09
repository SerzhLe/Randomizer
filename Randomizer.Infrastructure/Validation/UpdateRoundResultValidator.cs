using FluentValidation;
using Randomizer.Core.DTOs;

namespace Randomizer.Infrastructure.Validation;
internal class UpdateRoundResultValidator : AbstractValidator<UpdateRoundResultDto>
{
    public UpdateRoundResultValidator()
    {
        RuleFor(x => x.Score).InclusiveBetween(1, 5);

        RuleFor(x => x.Comment).MaximumLength(200);
    }
}
