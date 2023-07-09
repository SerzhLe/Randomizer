namespace Randomizer.Core.DTOs;

public record GameConfigDto
{
    public Guid Id { get; init; }

    public int DisplayId { get; init; }

    public int CountOfRounds { get; init; }

    public List<ParticipantDto> Participants { get; init; } = new();

    public List<MessageDto> Messages { get; init; } = new();
}
