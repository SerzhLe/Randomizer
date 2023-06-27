using Dapper;
using Randomizer.Core.Abstractions.Persistence;
using Randomizer.Domain.Entities;
using System.Data;

namespace Randomizer.Persistence.Dapper;
public class GameConfigRepository : IGameConfigRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly IDbTransaction _transaction;

    public GameConfigRepository(IDbConnection dbConnection, IDbTransaction transaction)
    {
        _dbConnection = dbConnection;
        _transaction = transaction;
    }

    public async Task AddAsync(GameConfigEntity entity)
    {
        var sqlGameConfig = "INSERT INTO game_config(game_config_id, count_of_rounds) VALUES(@Id, @CountOfRounds)";

        var commandGameConfig = new CommandDefinition(
            sqlGameConfig,
            new { entity.Id, entity.CountOfRounds },
            _transaction);

        await _dbConnection.ExecuteAsync(commandGameConfig);

        var sqlParticipants = "INSERT INTO participant(participant_id, nick_name, position, game_config_id) VALUES(@Id, @NickName, @Position, @GameConfigId)";

        var commandParticipants = new CommandDefinition(
            sqlParticipants,
            entity.Participants.Select(x => new { x.Id, x.NickName, x.Position, GameConfigId = x.StartGameConfigId }),
            _transaction);

        await _dbConnection.ExecuteAsync(sqlParticipants, commandParticipants);

        var sqlMessages = "INSERT INTO message(message_id, content, position, game_config_id) VALUES(@Id, @Content, @Position, @GameConfigId)";

        var commandMessages = new CommandDefinition(
            sqlMessages,
            entity.Messages.Select(x => new { x.Id, x.Content, x.Position, GameConfigId = x.StartGameConfigId }),
            _transaction);

        await _dbConnection.ExecuteAsync(sqlMessages, commandMessages);
    }

    public async Task<GameConfigEntity?> GetById(Guid id)
    {
        var sql = @"SELECT gc.game_config_id, gc.display_id, gc.count_of_rounds, p.participant_id, 
                    p.nick_name, p.position, m.message_id, m.content, m.position, 
                    gcr.game_config_round_id, gcr.is_completed, gcr.is_current,
                    gcrr.game_config_round_result_id, gcrr.score, gcrr.comment, gcrr.who_perform_action_id,
                    gcrr.who_perform_feedback_id, gcrr.message_id
                    FROM game_config gc
                    LEFT JOIN participant p ON p.game_config_id = gc.game_config_id,
                    LEFT JOIN message m ON m.game_config_id = gc.game_config_id,
                    LEFT JOIN game_config_round gcr ON gcr.game_config_id = gc.game_config_id,
                    LEFT JOIN game_config_round_result gcrr ON gcrr.game_config_round_id = gcr.game_config_round_id";

        var command = new CommandDefinition(sql, new { Id = id });

        var participantsLookup = new Dictionary<Guid, ParticipantEntity>();

        var gameConfig = await _dbConnection.QueryAsync<GameConfigEntity, ParticipantEntity, MessageEntity, RoundEntity, RoundResultEntity, GameConfigEntity>(
            command,
            (gameConfig, participant, message, round, result) =>
            {
                if (!participantsLookup.TryGetValue(participant.Id, out ParticipantEntity? value))
                {
                    participantsLookup.Add(participant.Id, participant);
                }

                gameConfig.Participants.Add(participant);
                gameConfig.Messages.Add(message);
                gameConfig.Rounds.Add(round);

                return gameConfig;
            },
            splitOn: "p.participant_id, m.message_id, gcr.game_config_round_id, gcrr.game_config_round_result_id");


        var sqlRoundResults = @"SELECT gcrr.game_config_round_result_id, gcrr.who_perform_action_id,
                    gcrr.who_perform_feedback_id, gcrr.message_id, gcrrp1.position, gcrr2.position, gcrrm.position
                    FROM game_config_round_result gcrr
                    LEFT JOIN participant gcrrp1 ON gcrrp1.participant_id = gcrr.who_perform_action_id,                    LEFT JOIN participant gcrrp1 ON gcrrp1.participant_id = gcrr.who_perform_action_id,
                    LEFT JOIN participant gcrrp2 ON gcrrp2.participant_id = gcrr.who_perform_feedback_id,
                    LEFT JOIN message gcrrm ON gcrrm.message_id = gcrr.message_id
                    WHERE gc.game_config_id = @Id";

        var commandRoundResults = new CommandDefinition(sql, new { Id = id });

        await _dbConnection.QueryAsync<RoundResultEntity, ParticipantEntity, ParticipantEntity, MessageEntity, RoundResultEntity>(
            command, 
            (roundResult, whoPerformAction, whoPerformFeedback, message) =>
            {

            },
            splitOn: "")

    }
}
