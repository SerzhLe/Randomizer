using Randomizer.Domain.Common;

namespace Randomizer.Domain.Entities;

public class ParticipantEntity : IEntity
{
    public Guid Id { get; set; }

    public string NickName { get; set; } = string.Empty;

    public Guid StartGameConfigId { get; set; }

    public StartGameConfigEntity? StartGameConfig { get; set; }
}
