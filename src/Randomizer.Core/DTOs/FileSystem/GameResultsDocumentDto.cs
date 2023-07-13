using Randomizer.Application.DTOs;

namespace Randomizer.Application.DTOs.FileSystem; 

public record GameResultsDocumentDto : BaseDocumentDto
{
    public int DisplayId { get; init; }

    public int CountOfRounds { get; init; }

    public List<ParticipantDto> Participants { get; init; } = new List<ParticipantDto>();

    public List<MessageDto> Messages { get; init; } = new List<MessageDto>();

    public List<RoundDocumentDto> Rounds { get; init; } = new List<RoundDocumentDto>();

    public List<WinnerDto> Winners { get; set; } = new List<WinnerDto>();
}
