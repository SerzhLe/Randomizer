using Randomizer.Domain.Entities;

namespace Randomizer.Core.Abstractions.Persistence;

public interface IRoundRepository
{
    Task<RoundEntity> GetById(Guid id);

    Task AddAsync(RoundEntity entity);
}
