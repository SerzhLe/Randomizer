using Randomizer.Application.Abstractions.Infrastructure;

namespace Randomizer.Infrastructure;

public class RandomService : IRandomService
{
    public int GetRandomNumber(int minValue, int maxValue)
    {
        var random = new Random();

        return random.Next(minValue, maxValue);
    }
}