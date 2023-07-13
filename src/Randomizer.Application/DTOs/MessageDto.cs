namespace Randomizer.Application.DTOs;

public record MessageDto
{
    public Guid Id { get; init; }

    public string Content { get; init; } = string.Empty;

    public int Position { get; init; }
}
