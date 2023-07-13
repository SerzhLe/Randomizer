namespace Randomizer.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    IGameConfigRepository GameConfigRepository { get; }

    IRoundRepository RoundRepository { get; }

    IRoundResultRepository RoundResultRepository { get; }

    Task SaveChangesAsync();
}

