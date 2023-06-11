namespace Randomizer.Core.DTOs; 

public record CreateParticipantDto 
{
    public string NickName { get; init; } = string.Empty;
}
