using Randomizer.Core.Abstractions.Persistence;
using Randomizer.Domain.Entities;
using System.Data;

namespace Randomizer.Persistence.Dapper;
public class RoundRepository : IRoundRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly IDbTransaction _transaction;

    public RoundRepository(IDbConnection dbConnection, IDbTransaction transaction)
    {
        _dbConnection = dbConnection;
        _transaction = transaction;
    }

    public Task AddAsync(RoundEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<RoundEntity> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}
