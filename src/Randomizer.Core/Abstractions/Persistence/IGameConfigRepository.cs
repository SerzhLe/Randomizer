using Randomizer.Core.DTOs;
using Randomizer.Domain.Entities;

namespace Randomizer.Core.Abstractions.Persistence;

public interface IGameConfigRepository
{
    Task<GameConfigEntity?> FindAsync(Guid id);

    Task<GameConfigEntity?> GetFullAsync(Guid id);

    Task<GameConfigEntity> AddAsync(GameConfigEntity entity);
}
