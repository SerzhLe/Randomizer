using Randomizer.Domain.Entities;

namespace Randomizer.Application.Abstractions.Persistence;

/// <summary>
/// Service that handles round data manipulation with db.
/// </summary>
public interface IRoundRepository
{
    /// <summary>
    /// Perform a lookup into db for a single round entity.
    /// </summary>
    /// <param name="id">Entity id.</param>
    /// <returns>Round data without its child entities.</returns>
    Task<RoundEntity?> GetByIdAsync(Guid id);

    /// <summary>
    /// Perform insert operation of a single round entity without any child entities.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>Newly created round entity.</returns>
    Task<RoundEntity> AddAsync(RoundEntity entity);

    /// <summary>
    /// Perform update operation of a single round entity without any child entities.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    Task UpdateAsync(RoundEntity entity);

    /// <summary>
    /// Perform a lookup into db for all round entity within specific game config id.
    /// </summary>
    /// <param name="gameConfigId">Game config id.</param>
    /// <returns>List of found rounds.</returns>
    Task<List<RoundEntity>> GetAllByGameConfigId(Guid gameConfigId);
}
