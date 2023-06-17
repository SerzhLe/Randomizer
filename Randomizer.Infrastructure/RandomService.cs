using Randomizer.Core.Abstractions.Infrastructure;

namespace Randomizer.Infrastructure;
public class RandomService : IRandomService
{
    private readonly Random _random;

    public RandomService()
    {
        _random = new Random();
    }

    public int GetRandomNumber(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }
}