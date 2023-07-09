using FluentValidation;
using Randomizer.Core.DTOs;

namespace Randomizer.Infrastructure.Validation;
internal class CreateGameConfigValidator : AbstractValidator<CreateGameConfigDto>
{
	public CreateGameConfigValidator()
	{
		RuleFor(x => x.CountOfRounds).NotEmpty().LessThanOrEqualTo(x => x.Messages.Count);

		RuleForEach(x => x.Participants).ChildRules(x => x.RuleFor(x => x.NickName).NotEmpty());

		RuleForEach(x => x.Messages).ChildRules(x => x.RuleFor(x => x.Content).NotEmpty());
    }
}