using Randomizer.Domain.Common;

namespace Randomizer.Domain.Entities;

public class StartGameConfigEntity : IEntity
{
    public Guid Id { get; set; }

    public int DisplayId { get; set; }

    public int CountOfRounds { get; set; }

    public List<ParticipantEntity> Participants { get; set; } = new();

    public List<MessageEntity> Messages { get; set; } = new();
}
