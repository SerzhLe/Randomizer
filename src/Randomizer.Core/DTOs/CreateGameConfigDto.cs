namespace Randomizer.Application.DTOs;

public record CreateGameConfigDto
{
    public int CountOfRounds { get; init; }

    public List<string> Participants { get; init; } = new();

    public List<string> Messages { get; init; } = new();
}
