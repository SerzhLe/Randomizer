using Randomizer.Domain.Entities;

namespace Randomizer.Application.Abstractions.Persistence;

/// <summary>
/// Service that handles game config data manipulation with db.
/// </summary>
public interface IGameConfigRepository
{
    /// <summary>
    /// Perform a lookup into db for a single game config entity.
    /// </summary>
    /// <param name="id">Entity id.</param>
    /// <returns>Game config data without its child entities.</returns>
    Task<GameConfigEntity?> FindAsync(Guid id);

    /// <summary>
    /// Perform a lookup into db for a single game config entity.
    /// </summary>
    /// <param name="id">Entity id.</param>
    /// <returns>Game config data with all child entities.</returns>
    Task<GameConfigEntity?> GetFullAsync(Guid id);

    /// <summary>
    /// Perform insert operation of a single game config entity with participants and messages.
    /// </summary>
    /// <param name="entity">Entity id.</param>
    /// <returns>Newly created game config entity with participants and messages.</returns>
    Task<GameConfigEntity> AddAsync(GameConfigEntity entity);
}
