using FluentValidation;
using Randomizer.Core.DTOs;

namespace Randomizer.Infrastructure.Validation;
internal class CreateGameConfigValidator : AbstractValidator<CreateGameConfigDto>
{
	public CreateGameConfigValidator()
	{
		RuleFor(x => x.CountOfRounds).NotEmpty().LessThanOrEqualTo(x => x.Messages.Count);

		RuleForEach(x => x.Participants).NotEmpty();

		RuleForEach(x => x.Messages).NotEmpty();

		RuleFor(x => x.Messages.Count).GreaterThan(x => x.CountOfRounds).OverridePropertyName(x => x.Messages);
    }
}