namespace Randomizer.Application.DTOs;

public record WinnerDto
{
    public Guid Id { get; init; }

    public double TotalScore { get; init; }
}
