using Randomizer.Domain.Common;

namespace Randomizer.Domain.Entities;

public class MessageEntity : IEntity
{
    public Guid Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public Guid StartGameConfigId { get; set; }

    public StartGameConfigEntity? StartGameConfig { get; set; }
}
