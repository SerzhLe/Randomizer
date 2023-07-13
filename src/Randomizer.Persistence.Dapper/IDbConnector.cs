using System.Data;

namespace Randomizer.Persistence.Dapper;

public interface IDbConnector
{
    IDbConnection CreateConnection();
}
