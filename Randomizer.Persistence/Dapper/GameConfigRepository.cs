using Randomizer.Core.Abstractions.Persistence;
using Randomizer.Domain.Entities;
using System.Data;

namespace Randomizer.Persistence.Dapper;
public class GameConfigRepository : IGameConfigRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly IDbTransaction _transaction;

    public GameConfigRepository(IDbConnection dbConnection, IDbTransaction transaction)
    {
        _dbConnection = dbConnection;
        _transaction = transaction;
    }

    public Task AddAsync(GameConfigEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<GameConfigEntity?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<GameConfigEntity?> GetLastCreated()
    {
        throw new NotImplementedException();
    }
}
