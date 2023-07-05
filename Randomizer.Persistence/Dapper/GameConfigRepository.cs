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
        entity.Messages.ForEach(x => x.Id = Guid.NewGuid());
        entity.Participants.ForEach(x => x.Id = Guid.NewGuid());

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

        return entity;
    }

    public async Task<GameConfigEntity?> GetById(Guid id)
    {
        //var sql = @"SELECT gc.game_config_id, gc.display_id, gc.count_of_rounds, p.participant_id, 
        //            p.nick_name, p.position, m.message_id, m.content, m.position, 
        //            gcr.game_config_round_id, gcr.is_completed, gcr.is_current,
        //            gcrr.game_config_round_result_id, gcrr.score, gcrr.comment, gcrr.who_perform_action_id,
        //            gcrr.who_perform_feedback_id, gcrr.message_id
        //            FROM game_config gc
        //            LEFT JOIN participant p ON p.game_config_id = gc.game_config_id
        //            LEFT JOIN message m ON m.game_config_id = gc.game_config_id
        //            LEFT JOIN game_config_round gcr ON gcr.game_config_id = gc.game_config_id
        //            LEFT JOIN game_config_round_result gcrr ON gcrr.game_config_round_id = gcr.game_config_round_id
        //            WHERE gc.game_config_id = @Id";

        //var command = new CommandDefinition(sql, new { Id = id });

        //var participantsLookup = new Dictionary<Guid, ParticipantEntity>();
        //var messagesLookup = new Dictionary<Guid, MessageEntity>();
        //var roundsLookup = new Dictionary<Guid, RoundEntity>();
        //var roundResultLookup = new Dictionary<Guid, RoundResultEntity>();

        //var gameConfig = await _dbConnection.QueryAsync<GameConfigEntity, ParticipantEntity, MessageEntity, RoundEntity, RoundResultEntity, GameConfigEntity>(
        //    command,
        //    (gameConfig, participant, message, round, roundResult) =>
        //    {
        //        if (!participantsLookup.TryGetValue(participant.Id, out var participantValue))
        //        {
        //            participantsLookup.Add(participant.Id, participant);
        //            gameConfig.Participants.Add(participant);
        //        }

        //        if (!messagesLookup.TryGetValue(message.Id, out var messageValue))
        //        {
        //            messagesLookup.Add(message.Id, message);
        //            gameConfig.Messages.Add(message);
        //        }

        //        if (!roundsLookup.TryGetValue(round.Id, out var roundValue))
        //        {
        //            roundsLookup.Add(round.Id, round);
        //            gameConfig.Rounds.Add(round);
        //        }

        //        if (!roundResultLookup.TryGetValue(roundResult.Id, out var roundResultValue))
        //        {
        //            roundResultLookup.Add(roundResult.Id, roundResult);
        //            round.RoundResults.Add(roundResult);
        //        }

        //        return gameConfig;
        //    },
        //    splitOn: "p.participant_id, m.message_id, gcr.game_config_round_id, gcrr.game_config_round_result_id");

        //var game = gameConfig.First();

        //var sqlRoundResults = $@"SELECT gcrr.game_config_round_result_id, gcrr.who_perform_action_id,
        //            gcrr.who_perform_feedback_id, gcrr.message_id, gcrrp1.position, gcrr2.position, gcrrm.position
        //            FROM game_config_round_result gcrr
        //            LEFT JOIN participant gcrrp1 ON gcrrp1.participant_id = gcrr.who_perform_action_id,
        //            LEFT JOIN participant gcrrp2 ON gcrrp2.participant_id = gcrr.who_perform_feedback_id,
        //            LEFT JOIN message gcrrm ON gcrrm.message_id = gcrr.message_id
        //            WHERE gcrr.round_id IN({string.Join(", ", game.Rounds.Select(x => $"'{x.Id}'"))})";

        //var commandRoundResults = new CommandDefinition(sql, new { Id = id });

        ////var roundWhoPerformActionLookup1 = new Dictionary<Guid, ParticipantEntity>();
        ////var roundWhoPerformFeedbackLookup2 = new Dictionary<Guid, ParticipantEntity>();
        ////var roundMessagesLookup = new Dictionary<Guid, MessageEntity>();

        //var roundResults = await _dbConnection.QueryAsync<RoundResultEntity, ParticipantEntity, ParticipantEntity, MessageEntity, RoundResultEntity>(
        //    command,
        //    (roundResult, whoPerformAction, whoPerformFeedback, message) =>
        //    {
        //        roundResult.WhoPerformAction = whoPerformAction;
        //        roundResult.WhoPerformFeedback = whoPerformFeedback;
        //        roundResult.Message = message;

        //        return roundResult;
        //    },
        //    splitOn: "gcrrp1.position, gcrr2.position, gcrrm.position");


        //foreach (var round in game.Rounds)
        //{
        //    round.RoundResults.AddRange(roundResults.Where(x => x.RoundId == round.Id));
        //}

        var sql = "SELECT game_config_id Id, display_id DisplayId, count_of_rounds CountOfRounds  FROM game_config WHERE game_config_id = @ID";

        var command = new CommandDefinition(sql, new { id });

        return (await _dbConnection.QueryAsync<GameConfigEntity>(command)).SingleOrDefault();
    }

    public async Task<List<GameConfigEntity>> GetConfig()
    {
        return (await _dbConnection.QueryAsync<GameConfigEntity>("SELECT game_config_id Id, display_id DisplayId, count_of_rounds CountOfRounds  FROM game_config")).ToList();
    }
}
