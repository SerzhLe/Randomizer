namespace Randomizer.Core.DTOs;

public record RoundResultDto
{
    public Guid Id { get; init; }

    public double? Score { get; init; }

    public string? Comment { get; init; }

    public bool LastRoundResult { get; init; }

    public ParticipantDto? WhoPerformAction { get; init; }

    public ParticipantDto? WhoPerformFeedback { get; init; }

    public MessageDto? Message { get; init; }
}
