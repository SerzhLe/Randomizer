namespace Randomizer.Application.Abstractions.Persistence;

/// <summary>
/// Handle lifecycle of repositories and maintain single operation transaction.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Game Config Repository.
    /// </summary>
    IGameConfigRepository GameConfigRepository { get; }

    /// <summary>
    /// Round Repository.
    /// </summary>
    IRoundRepository RoundRepository { get; }

    /// <summary>
    /// Round Result Repository.
    /// </summary>
    IRoundResultRepository RoundResultRepository { get; }

    /// <summary>
    /// Commit changes to db.
    /// </summary>
    Task SaveChangesAsync();
}

