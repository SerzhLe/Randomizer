using Dapper;
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

    public async Task<RoundEntity> AddAsync(RoundEntity entity)
    {
        entity.Id = Guid.NewGuid();

        var sql = "INSERT INTO game_config_round(game_config_round_id, is_completed, is_current, game_config_id) VALUES(@Id, @IsCompleted, @IsCurrent, @GameConfigId)";

        var command = new CommandDefinition(sql, new { entity.Id, entity.IsCompleted, entity.IsCurrent, entity.GameConfigId }, transaction: _transaction);

        await _dbConnection.ExecuteAsync(command);

        return entity;
    }

    public async Task<RoundEntity?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT game_config_round_id Id, is_completed IsCompleted, is_current IsCurrent, game_config_id GameConfigId FROM game_config_round WHERE game_config_round_id = @Id";

        var command = new CommandDefinition(sql, new { id }, transaction: _transaction);

        return (await _dbConnection.QueryAsync<RoundEntity>(command)).SingleOrDefault();
    }

    public async Task UpdateAsync(RoundEntity entity)
    {
        var sql = "UPDATE game_config_round SET is_completed = @IsCompleted, is_current = @IsCurrent WHERE game_config_round_id = @Id";

        var command = new CommandDefinition(sql, new { entity.IsCompleted, entity.IsCurrent, entity.Id }, transaction: _transaction);

        await _dbConnection.ExecuteAsync(command);
    }
}
