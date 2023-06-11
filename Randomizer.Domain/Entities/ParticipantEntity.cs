using Randomizer.Domain.Common;

namespace Randomizer.Domain.Entities;

public class ParticipantEntity : IEntity
{
    public Guid Id { get; set; }

    public string NickName { get; set; } = string.Empty;

    public int Position { get; set; }

    public Guid StartGameConfigId { get; set; }

    public GameConfigEntity? StartGameConfig { get; set; }
}
