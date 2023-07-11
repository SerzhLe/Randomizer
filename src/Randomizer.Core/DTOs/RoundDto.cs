namespace Randomizer.Core.DTOs;
public record RoundDto
{
    public Guid Id { get; init; }

    public bool IsCurrent { get; init; }

    public bool IsCompleted { get; init; }

    public int SequenceNumber { get; init; }

    public Guid GameConfigId { get; init; }

    public bool LastRound { get; init; }
}
