using Randomizer.Core.Abstractions.Persistence;
using System.Data;

namespace Randomizer.Persistence.Dapper;
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private IDbConnection _dbConnection;
    private IDbTransaction _transaction;
    private IGameConfigRepository _gameConfigRepository;
    private IRoundRepository _roundRepository;

    public UnitOfWork(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
        _dbConnection.Open();
        _transaction = _dbConnection.BeginTransaction();
    }


    public IGameConfigRepository GameConfigRepository
    {
        get
        {
            return _gameConfigRepository ??= new GameConfigRepository(_dbConnection, _transaction);
        }
    }


    public IRoundRepository RoundRepository
    {
        get
        {
            return _roundRepository ??= new RoundRepository(_dbConnection, _transaction);
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await Task.Run(() => _transaction.Commit());
        }
        catch (Exception ex)
        {
            _transaction.Rollback();
        }
    }

    public void Dispose()
    {
        _dbConnection?.Close();
        _dbConnection?.Dispose();
        _transaction?.Dispose();
    }
}
