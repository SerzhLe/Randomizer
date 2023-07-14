using FluentValidation;
using Randomizer.Application.DTOs;

namespace Randomizer.Application.Validation;

public class CreateGameConfigValidator : AbstractValidator<CreateGameConfigDto>
{
	public CreateGameConfigValidator()
	{
		RuleFor(x => x.CountOfRounds).GreaterThan(0).NotEmpty().LessThanOrEqualTo(x => x.Messages.Count);

		RuleFor(x => x.Participants).NotEmpty();

		RuleForEach(x => x.Participants).NotEmpty();

		RuleForEach(x => x.Messages).NotEmpty();

		RuleFor(x => x.Messages.Count).GreaterThan(x => x.CountOfRounds).OverridePropertyName(x => x.Messages);

    }
}