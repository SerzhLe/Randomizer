namespace Randomizer.Application.DTOs;

public record GameResultsDto
{
    public Guid GameId { get; init; }

    public int CountOfRounds { get; init; }

    public List<WinnerDto> Winners { get; init; } = new List<WinnerDto>();
}
