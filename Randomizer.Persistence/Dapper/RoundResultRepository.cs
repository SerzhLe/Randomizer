using Dapper;
using Randomizer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Persistence.Dapper
{
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

            var command = new CommandDefinition(sql, entity, _transaction);

            await _dbConnection.ExecuteAsync(command);

            return entity;
        }
    }
}
