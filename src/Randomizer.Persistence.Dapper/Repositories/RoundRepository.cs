using Dapper;
using Randomizer.Application.Abstractions.Persistence;
using Randomizer.Domain.Entities;
using System.Data;

namespace Randomizer.Persistence.Dapper.Repositories;
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
        var sqlExistingRoundsCount = @"SELECT COUNT(game_config_round_id) FROM game_config_round
                                       WHERE game_config_id = @GameConfigId";

        var existingRoundsCount = (await _dbConnection.QueryAsync<int>(
            sqlExistingRoundsCount,
            new { entity.GameConfigId },
            _transaction)).SingleOrDefault();

        entity.Id = Guid.NewGuid();
        entity.SequenceNumber = existingRoundsCount + 1;

        var sql = @"INSERT INTO game_config_round(game_config_round_id, is_completed, is_current, sequence_number, game_config_id) 
                    VALUES(@Id, @IsCompleted, @IsCurrent, @SequenceNumber, @GameConfigId)";

        await _dbConnection.ExecuteAsync(sql, entity, _transaction);

        return entity;
    }

    public async Task<RoundEntity?> GetByIdAsync(Guid id)
    {
        var sql = @"SELECT game_config_round_id Id, is_completed IsCompleted, is_current IsCurrent, sequence_number SequenceNumber
                    game_config_id GameConfigId FROM game_config_round WHERE game_config_round_id = @Id";

        return (await _dbConnection.QueryAsync<RoundEntity>(sql, new { id }, _transaction)).SingleOrDefault();
    }

    public async Task UpdateAsync(RoundEntity entity)
    {
        var sql = @"UPDATE game_config_round SET is_completed = @IsCompleted, is_current = @IsCurrent 
                    WHERE game_config_round_id = @Id";

        await _dbConnection.ExecuteAsync(sql, entity, _transaction);
    }

    public async Task<List<RoundEntity>> GetAllByGameConfigId(Guid gameConfigId)
    {
        var sql = @"SELECT game_config_round_id Id, is_completed IsCompleted, is_current IsCurrent, sequence_number SequenceNumber, 
                    game_config_id GameConfigId FROM game_config_round WHERE game_config_id = @Id";

        return (await _dbConnection.QueryAsync<RoundEntity>(sql, new { Id = gameConfigId }, _transaction)).ToList();
    }
}
