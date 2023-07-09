using Randomizer.Common;

namespace Randomizer.Core.Abstractions.Infrastructure;
public interface ICoreValidator
{
    ValidationResult Validate<T>(T dto);
}