using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Randomizer.Persistence.Dapper;
public class DbConnector : IDbConnector
{
	private readonly string? _connectionString;

	//public DbConnector(IConfiguration configuration)
	//{
	//	_connectionString = configuration.GetConnectionString("DbConnection");
	//}

	public DbConnector(string connection)
	{
        _connectionString = connection;

    }

	public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}
