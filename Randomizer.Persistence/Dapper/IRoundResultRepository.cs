using Randomizer.Domain.Entities;

namespace Randomizer.Persistence.Dapper;
public interface IRoundResultRepository
{
    Task<RoundResultEntity> AddAsync(RoundResultEntity entity);
}
