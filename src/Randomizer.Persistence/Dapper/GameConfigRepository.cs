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

    public async Task<GameConfigEntity> AddAsync(GameConfigEntity entity)
    {
        entity.Id = Guid.NewGuid();
        entity.Messages
            .Select((x, i) =>
            {
                x.Id = Guid.NewGuid();
                x.Position = i;
                x.StartGameConfigId = entity.Id;
                return x;
            })
            .ToList();

        entity.Participants
            .Select((x, i) =>
            {
                x.Id = Guid.NewGuid();
                x.Position = i;
                x.StartGameConfigId = entity.Id;
                return x;
            })
            .ToList();

        var sqlGameConfig = "INSERT INTO game_config (game_config_id, count_of_rounds) VALUES (@Id, @CountOfRounds)";

        await _dbConnection.ExecuteAsync(sqlGameConfig, entity, _transaction);

        if (entity.Participants.Any())
        {
            var sqlParticipants = "INSERT INTO participant (participant_id, nick_name, position, game_config_id) VALUES (@Id, @NickName, @Position, @StartGameConfigId)";

            await _dbConnection.ExecuteAsync(sqlParticipants, entity.Participants, _transaction);
        }

        if (entity.Messages.Any())
        {
            var sqlMessages = "INSERT INTO message (message_id, content, position, game_config_id) VALUES (@Id, @Content, @Position, @StartGameConfigId)";

            await _dbConnection.ExecuteAsync(sqlMessages, entity.Messages, _transaction);
        }

        return entity;
    }

    public async Task<GameConfigEntity?> FindAsync(Guid id)
    {
        var sql = "SELECT game_config_id Id, display_id DisplayId, count_of_rounds CountOfRounds FROM game_config WHERE game_config_id = @ID";

        var command = new CommandDefinition(sql, new { id });

        return (await _dbConnection.QueryAsync<GameConfigEntity>(command)).SingleOrDefault();
    }

    public async Task<GameConfigEntity?> GetFullAsync(Guid id)
    {
        var sql = @"SELECT gc.game_config_id Id, gc.display_id DisplayId, gc.count_of_rounds CountOfRounds, p.participant_id Id, 
                    p.nick_name NickName, p.position Position, m.message_id Id, m.content Content, m.position Position, 
                    gcr.game_config_round_id Id, gcr.is_completed IsCompleted, gcr.is_current IsCurrent
                    FROM game_config gc
                    LEFT JOIN participant p ON p.game_config_id = gc.game_config_id
                    LEFT JOIN message m ON m.game_config_id = gc.game_config_id
                    LEFT JOIN game_config_round gcr ON gcr.game_config_id = gc.game_config_id
                    WHERE gc.game_config_id = @Id";

        var participantsLookup = new Dictionary<Guid, ParticipantEntity>();
        var messagesLookup = new Dictionary<Guid, MessageEntity>();
        var roundsLookup = new Dictionary<Guid, RoundEntity>();
        GameConfigEntity game = null!;

        var gameConfig = await _dbConnection.QueryAsync<GameConfigEntity, ParticipantEntity, MessageEntity, RoundEntity, GameConfigEntity>(
            sql,
            (gameConfig, participant, message, round) =>
            {
                if (game is null)
                {
                    game = gameConfig;
                }

                if (participant is not null && !participantsLookup.TryGetValue(participant.Id, out var participantValue))
                {
                    participantsLookup.Add(participant.Id, participant);
                    game.Participants.Add(participant);
                }

                if (message is not null && !messagesLookup.TryGetValue(message.Id, out var messageValue))
                {
                    messagesLookup.Add(message.Id, message);
                    game.Messages.Add(message);
                }

                if (round is not null && !roundsLookup.TryGetValue(round.Id, out var roundValue))
                {
                    roundsLookup.Add(round.Id, round);
                    game.Rounds.Add(round);
                }

                return gameConfig;
            },
            param: new { id },
            splitOn: "Id, Id, Id");

        var roundIds = game.Rounds
            .Select((x, i) => ($"@Id{i}", x.Id))
            .ToDictionary(x => x.Item1, x => (object)x.Id);

        var param = new DynamicParameters(roundIds);

        var sqlRoundResults = $@"SELECT gcrr.game_config_round_result_id Id, gcrr.who_perform_action_id WhoPerformActionId,
                    gcrr.who_perform_feedback_id WhoPerformFeedbackId, gcrr.message_id MessageId,
                    gcrr.comment Comment, gcrr.score Score, gcrr.game_config_round_id RoundId,
                    gcrrp1.participant_id Id, gcrrp1.nick_name NickName, gcrrp1.position Position, 
                    gcrrp2.participant_id Id, gcrrp2.nick_name NickName, gcrrp2.position Position, 
                    gcrrm.message_id Id, gcrrm.content Content, gcrrm.position Position
                    FROM game_config_round_result gcrr
                    LEFT JOIN participant gcrrp1 ON gcrrp1.participant_id = gcrr.who_perform_action_id
                    LEFT JOIN participant gcrrp2 ON gcrrp2.participant_id = gcrr.who_perform_feedback_id
                    LEFT JOIN message gcrrm ON gcrrm.message_id = gcrr.message_id
                    WHERE gcrr.game_config_round_id IN({string.Join(", ", roundIds.Select(x => x.Key).ToList())})";

        var roundResults = await _dbConnection.QueryAsync<RoundResultEntity, ParticipantEntity, ParticipantEntity, MessageEntity, RoundResultEntity>(
            sqlRoundResults,
            (roundResult, whoPerformAction, whoPerformFeedback, message) =>
            {
                roundResult.WhoPerformAction = whoPerformAction;
                roundResult.WhoPerformFeedback = whoPerformFeedback;
                roundResult.Message = message;

                return roundResult;
            },
            param: param,
            splitOn: "Id, Id, Id");

        foreach (var round in game.Rounds)
        {
            round.RoundResults.AddRange(roundResults.Where(x => x.RoundId == round.Id));
        }

        return game;
    }
}
