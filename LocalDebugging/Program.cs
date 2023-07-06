using Randomizer.Core.DTOs;
using Randomizer.Domain.Entities;
using Randomizer.Persistence.Dapper;

var connectionString = "Host=localhost:5430;Database=skill_up;Username=postgres;Password=kwxzqa2369475;";

var dbConnector = new DbConnector(connectionString);

using var uow = new UnitOfWork(dbConnector);

//var result = await uow.GameConfigRepository.GetById(Guid.Parse("cba882c2-5fb9-41f1-9ea3-3238c6d90dc2"));

//var result = await uow.GameConfigRepository.GetConfig();

//var participant = await uow.ParticipantRepository.AddAsync(new Randomizer.Domain.Entities.ParticipantEntity
//{
//    NickName = "Serzh",
//    Position = 0,
//    StartGameConfigId = Guid.Parse("cba882c2-5fb9-41f1-9ea3-3238c6d90dc2")
//});

//var game = await uow.GameConfigRepository.GetById(Guid.Parse("cba882c2-5fb9-41f1-9ea3-3238c6d90dc2"));

//var round = await uow.RoundRepository.GetByIdAsync(Guid.Parse("38ff2fc7-20b7-42f3-a559-be63d9ec2379"));

//round.IsCurrent = true;
//round.IsCompleted = true;

//await uow.RoundRepository.UpdateAsync(new Randomizer.Domain.Entities.RoundEntity
//{
//    Id = round.Id,
//    IsCurrent = true,
//    IsCompleted = true
//});

//var r = await uow.RoundRepository.AddAsync(new Randomizer.Domain.Entities.RoundEntity
//{
//    IsCurrent = false,
//    IsCompleted = true,
//    GameConfigId = game.Id
//});

//var r = new RoundResultEntity
//{
//    Comment = "sdwd",
//    Score = 3,
//    MessageId = Guid.Parse("bb00b4a8-d8f0-4d9e-bd2e-0e1dceec98ad"),
//    WhoPerformActionId = Guid.Parse("7a2216c9-b7c8-4ad5-b7fe-68c7cc33b891"),
//    WhoPerformFeedbackId = Guid.Parse("7a2216c9-b7c8-4ad5-b7fe-68c7cc33b891"),
//    RoundId = Guid.Parse("38ff2fc7-20b7-42f3-a559-be63d9ec2379")
//};

//await uow.RoundResultRepository.AddAsync(r);

//var gameEntity = new GameConfigEntity
//{
//    CountOfRounds = 3,
//    Messages = new List<MessageEntity>
//    {
//        new MessageEntity { Content = "weded" },
//        new MessageEntity { Content = "DEFWFE" },
//        new MessageEntity { Content = "defWEFNEF*__EFW*EF" }
//    },
//    Participants = new List<ParticipantEntity>
//    {
//        new ParticipantEntity { NickName = "aFEFWF" },
//        new ParticipantEntity { NickName = "___EFEW#@" },
//        new ParticipantEntity { NickName = "23424DEWOF#@$" },
//        new ParticipantEntity { NickName = "aFEFWF" }
//    }
//};

//await uow.GameConfigRepository.AddAsync(gameEntity);

//var game = await uow.GameConfigRepository.GetFullAsync(Guid.Parse("cba882c2-5fb9-41f1-9ea3-3238c6d90dc2"));

//await uow.SaveChangesAsync();
Console.ReadKey();