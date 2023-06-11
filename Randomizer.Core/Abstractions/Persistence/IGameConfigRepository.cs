using Randomizer.Core.DTOs;
using Randomizer.Domain.Entities;

namespace Randomizer.Core.Abstractions.Persistence;

public interface IGameConfigRepository
{
    Task<GameConfigEntity?> GetById(Guid id);

    Task<GameConfigEntity?> GetLastCreated();

    void Add(GameConfigEntity entity);

    Task<int> SaveChangesAsync();
}
