using Randomizer.Core.Abstractions.Infrastructure;

namespace Randomizer.Infrastructure;
internal class RandomService : IRandomService
{
    private readonly Random _random;

    internal RandomService()
    {
        _random = new Random();
    }

    public int GetRandomNumber(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }
}