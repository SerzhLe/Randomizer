using Randomizer.Core.DTOs;
using Randomizer.Domain.Entities;

namespace Randomizer.Core.Abstractions.Persistence;

public interface IGameConfigRepository
{
    Task<GameConfigEntity?> GetById(Guid id);

    Task<GameConfigEntity> AddAsync(GameConfigEntity entity);

    Task<List<GameConfigEntity>> GetConfig();
}
