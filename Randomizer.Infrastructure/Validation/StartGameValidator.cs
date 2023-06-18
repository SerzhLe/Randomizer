using FluentValidation;
using Randomizer.Core.DTOs;

namespace Randomizer.Infrastructure.Validation;
public class StartGameValidator : AbstractValidator<CreateGameConfigDto>
{
	public StartGameValidator()
	{
		RuleFor(x => x.CountOfRounds).NotEmpty().LessThanOrEqualTo(x => x.Messages.Count);

		RuleForEach(x => x.Participants).ChildRules(x => x.RuleFor(x => x.NickName).NotEmpty());

		RuleForEach(x => x.Messages).ChildRules(x => x.RuleFor(x => x.Content).NotEmpty());
    }
}