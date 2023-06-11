namespace Randomizer.Core.DTOs; 

public record CreateMessageDto 
{
    public string Content { get; init; } = string.Empty;
}
