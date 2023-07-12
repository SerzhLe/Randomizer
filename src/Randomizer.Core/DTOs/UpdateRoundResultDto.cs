namespace Randomizer.Core.DTOs;

public record UpdateRoundResultDto
{
    public Guid Id { get; init; }

    public double Score { get; init; }

    public string? Comment { get; init; }
}
