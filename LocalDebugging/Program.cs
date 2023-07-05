using Randomizer.Persistence.Dapper;

var connectionString = "Host=localhost:5430;Username=postgres;Password=kwxzqa2369475";

var dbConnector = new DbConnector(connectionString);

var uow = new UnitOfWork(dbConnector);

var result = await uow.GameConfigRepository.GetById(Guid.Parse("cba882c2-5fb9-41f1-9ea3-3238c6d90dc2"));