using Randomizer.Application.DTOs;
using Randomizer.Domain.Entities;

namespace Randomizer.Application.Abstractions.Persistence;

public interface IGameConfigRepository
{
    Task<GameConfigEntity?> FindAsync(Guid id);

    Task<GameConfigEntity?> GetFullAsync(Guid id);

    Task<GameConfigEntity> AddAsync(GameConfigEntity entity);
}
