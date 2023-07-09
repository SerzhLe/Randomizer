using FluentValidation;
using Randomizer.Core.DTOs;

namespace Randomizer.Infrastructure.Validation
{
    internal class UpdateRoundResultValidator : AbstractValidator<UpdateRoundResultDto>
    {
        internal UpdateRoundResultValidator()
        {
            RuleFor(x => x.Score).NotEmpty().GreaterThan(0);

            RuleFor(x => x.Comment).MaximumLength(200);
        }
    }
}
