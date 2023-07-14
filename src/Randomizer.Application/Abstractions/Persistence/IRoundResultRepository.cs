using Randomizer.Domain.Entities;

namespace Randomizer.Application.Abstractions.Persistence;

/// <summary>
/// Service that handles round result data manipulation with db.
/// </summary>
public interface IRoundResultRepository
{
    /// <summary>
    /// Perform insert operation of a single round result entity.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>Newly created round result entity.</returns>
    Task<RoundResultEntity> AddAsync(RoundResultEntity entity);

    /// <summary>
    /// Perform update operation of a single round result entity without any child entities.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    Task UpdateAsync(RoundResultEntity entity);

    /// <summary>
    /// Perform a lookup into db for a single round result entity.
    /// </summary>
    /// <param name="id">Entity id.</param>
    /// <returns>Round result data without its child entities.</returns>
    Task<RoundResultEntity?> FindAsync(Guid id);
}
