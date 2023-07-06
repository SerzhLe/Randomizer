using Randomizer.Domain.Entities;

namespace Randomizer.Core.Abstractions.Persistence;
public interface IRoundResultRepository
{
    Task<RoundResultEntity> AddAsync(RoundResultEntity entity);
}
