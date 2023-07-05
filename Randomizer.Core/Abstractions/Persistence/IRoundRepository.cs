using Randomizer.Domain.Entities;

namespace Randomizer.Core.Abstractions.Persistence;

public interface IRoundRepository
{
    Task<RoundEntity?> GetByIdAsync(Guid id);

    Task<RoundEntity> AddAsync(RoundEntity entity);

    Task UpdateAsync(RoundEntity entity);
}
