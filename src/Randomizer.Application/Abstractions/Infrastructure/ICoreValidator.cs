using Randomizer.Common;

namespace Randomizer.Application.Abstractions.Infrastructure;
public interface ICoreValidator
{
    ValidationResult Validate<T>(T dto);
}