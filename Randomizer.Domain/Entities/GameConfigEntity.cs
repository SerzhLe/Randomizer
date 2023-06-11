using Randomizer.Domain.Common;

namespace Randomizer.Domain.Entities;

public class GameConfigEntity : IEntity
{
    public Guid Id { get; set; }

    public int DisplayId { get; set; }

    public int CountOfRounds { get; set; }

    public List<ParticipantEntity> Participants { get; set; } = new();

    public List<MessageEntity> Messages { get; set; } = new();

    public List<RoundEntity> Rounds { get; set; } = new();
}
