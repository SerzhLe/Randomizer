using Randomizer.Domain.Common;

namespace Randomizer.Domain.Entities;

public class RoundEntity : IEntity
{
    public Guid Id { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsCurrent { get; set; }

    public Guid GameConfigEntityId { get; set; }

    public GameConfigEntity? GameConfig { get; set; }

    public List<RoundResultEntity> RoundResults { get; set; } = new();
}
