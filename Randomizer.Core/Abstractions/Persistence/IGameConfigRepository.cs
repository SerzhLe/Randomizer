using Randomizer.Core.DTOs;
using Randomizer.Domain.Entities;

namespace Randomizer.Core.Abstractions.Persistence;

public interface IGameConfigRepository
{
    Task<GameConfigEntity?> GetById(Guid id);

    Task<GameConfigEntity?> GetLastCreated();

    Task AddAsync(GameConfigEntity entity);
}
