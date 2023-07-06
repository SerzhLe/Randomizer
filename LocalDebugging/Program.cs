using Randomizer.Persistence.Dapper;

var connectionString = "Host=localhost:5430;Database=skill_up;Username=postgres;Password=kwxzqa2369475;";

var dbConnector = new DbConnector(connectionString);

using var uow = new UnitOfWork(dbConnector);

//var result = await uow.GameConfigRepository.GetById(Guid.Parse("cba882c2-5fb9-41f1-9ea3-3238c6d90dc2"));

var result = await uow.GameConfigRepository.GetConfig();

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


var game = await uow.GameConfigRepository.GetFullAsync(Guid.Parse("cba882c2-5fb9-41f1-9ea3-3238c6d90dc2"));

//await uow.SaveChangesAsync();
Console.WriteLine();