namespace Randomizer.Core.DTOs;

public record CreateGameConfigDto
{
    public int CountOfRounds { get; init; }

    public List<CreateParticipantDto> Participants { get; init; } = new();

    public List<CreateMessageDto> Messages { get; init; } = new();
}
