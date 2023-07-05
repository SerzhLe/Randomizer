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
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _transaction;

        public ParticipantRepository(IDbConnection dbConnection, IDbTransaction transaction)
        {
            _dbConnection = dbConnection;
            _transaction = transaction;
        }

        public async Task<ParticipantEntity> AddAsync(ParticipantEntity entity)
        {
            entity.Id = Guid.NewGuid();

            var sqlParticipant = "INSERT INTO participant(participant_id, nick_name, position, game_config_id) VALUES(@Id, @NickName, @Position, @GameConfigId)";

            var commandParticipant = new CommandDefinition(
                sqlParticipant,
                 new { entity.Id, entity.NickName, entity.Position, GameConfigId = entity.StartGameConfigId },
                _transaction);

            await _dbConnection.ExecuteAsync(commandParticipant);

            return entity;
        }

        public async Task<List<ParticipantEntity>> AddRangeAsync(List<ParticipantEntity> entities)
        {
            entities.ForEach(x => x.Id = Guid.NewGuid());

            var sqlParticipants = "INSERT INTO participant(participant_id, nick_name, position, game_config_id) VALUES(@Id, @NickName, @Position, @GameConfigId)";

            var commandParticipants = new CommandDefinition(
                sqlParticipants,
                entities.Select(x => new { x.Id, x.NickName, x.Position, GameConfigId = x.StartGameConfigId }),
                _transaction);

            await _dbConnection.ExecuteAsync(commandParticipants);

            return entities;
        }
    }
}
