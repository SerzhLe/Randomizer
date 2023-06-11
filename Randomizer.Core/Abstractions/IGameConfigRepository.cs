using Randomizer.Core.DTOs;
using Randomizer.Domain.Entities;

namespace Randomizer.Core.Abstractions;

public interface IGameConfigRepository
{
    Task<GameConfigEntity> GetById(Guid gameConfigId);

    Task<GameConfigEntity> GetLastCreated();

    void Add(GameConfigEntity gameConfig);

    Task<int> SaveChangesAsync();
}
