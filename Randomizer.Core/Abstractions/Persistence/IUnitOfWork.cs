namespace Randomizer.Core.Abstractions.Persistence;

public interface IUnitOfWork
{
    IGameConfigRepository GameConfigRepository { get; }

    IRoundRepository RoundRepository { get; }

    Task<int> SaveChangesAsync();

    Task RollBack();
}

