using Dapper;
using Randomizer.Core.Abstractions.Persistence;
using Randomizer.Domain.Entities;
using System.Data;

namespace Randomizer.Persistence.Dapper;
public class RoundResultRepository : IRoundResultRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly IDbTransaction _transaction;

    public RoundResultRepository(IDbConnection dbConnection, IDbTransaction transaction)
    {
        _dbConnection = dbConnection;
        _transaction = transaction;
    }

    public async Task<RoundResultEntity> AddAsync(RoundResultEntity entity)
    {
        entity.Id = Guid.NewGuid();

        var sql = @"INSERT INTO game_config_round_result (game_config_round_result_id, score, comment,
                    who_perform_action_id, who_perform_feedback_id, message_id, game_config_round_id) 
                    VALUES(@Id, @Score, @Comment, @WhoPerformActionId, @WhoPerformFeedbackId, 
                    @MessageId, @RoundId)";

        await _dbConnection.ExecuteAsync(sql, entity, _transaction);

        return entity;
    }

    public async Task UpdateAsync(RoundResultEntity entity)
    {
        var sql = @"UPDATE game_config_round_result 
                    SET score = @Score, comment = @Comment
                    WHERE game_config_round_result = @Id";

        await _dbConnection.ExecuteAsync(sql, entity, _transaction);
    }

    public async Task<RoundResultEntity?> FindAsync(Guid id)
    {
        var sql = @"SELECT game_config_round_result_id Id, score Score, comment Comment,
                    who_perform_action_id WhoPerformActionId, who_perform_feedback_id WhoPerformFeedbackId, 
                    message_id MessageId, game_config_round_id RoundId
                    FROM game_config_round_result WHERE game_config_round_result_id = @Id";

        var result = (await _dbConnection.QueryAsync<RoundResultEntity>(sql, id, _transaction)).SingleOrDefault();

        return result;
    }
}
