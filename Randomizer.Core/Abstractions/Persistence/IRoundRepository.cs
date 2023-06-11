using Randomizer.Domain.Entities;

namespace Randomizer.Core.Abstractions.Persistence;

public interface IRoundRepository
{
    Task<RoundEntity> GetById(Guid id);

    void Add(RoundEntity entity);

    Task<int> SaveChangesAsync();
}
