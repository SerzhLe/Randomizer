namespace Randomizer.Application.DTOs;

public record ParticipantDto
{
    public Guid Id { get; init; }

    public string NickName { get; init; } = string.Empty;

    public int Position { get; init; }
}

