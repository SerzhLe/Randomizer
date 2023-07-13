using Randomizer.Application.DTOs;

namespace Randomizer.Application.DTOs.FileSystem;

public record RoundDocumentDto
{
    public Guid Id { get; init; }

    public int SequenceNumber { get; init; }

    public List<RoundResultDto> RoundResults { get; init; } = new List<RoundResultDto>();
}
