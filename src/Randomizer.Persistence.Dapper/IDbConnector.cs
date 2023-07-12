using System.Data;

namespace Randomizer.Dapper;
public interface IDbConnector
{
    IDbConnection CreateConnection();
}
