using Randomizer.Domain.Entities;

namespace Randomizer.Persistence.Dapper;
public interface IParticipantRepository
{
    Task<ParticipantEntity> AddAsync(ParticipantEntity entity);

    Task<List<ParticipantEntity>> AddRangeAsync(List<ParticipantEntity> entities);
}
