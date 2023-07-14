using System.Data;

namespace Randomizer.Persistence;

public interface IDbConnector
{
    IDbConnection CreateConnection();
}
