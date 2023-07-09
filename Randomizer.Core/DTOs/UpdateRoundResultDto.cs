namespace Randomizer.Core.DTOs;
public record UpdateRoundResultDto
{
    public Guid Id { get; init; }

    public int Score { get; init; }

    public string? Comment { get; init; }
}
