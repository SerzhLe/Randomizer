namespace Randomizer.Application.Abstractions.Infrastructure;

/// <summary>
/// Service that generate random number.
/// </summary>
public interface IRandomService
{
    /// <summary>
    /// Generate random number in range.
    /// </summary>
    /// <param name="minValue">The inclusive lower bound of random number.</param>
    /// <param name="maxValue">The exclusive upper bound of random number.</param>
    /// <returns></returns>
    int GetRandomNumber(int minValue, int maxValue);
}
