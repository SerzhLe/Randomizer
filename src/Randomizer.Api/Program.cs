using Randomizer.Api.Extensions;
using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Application.Services;
using Randomizer.Application.Services.DocumentDataFetchers;
using Randomizer.Infrastructure;
using Randomizer.Persistence.Dapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddScoped<IGameProcessorService, GameProcessorService>();
builder.Services.AddScoped<IDocumentDataFetcher<GameResultsDocumentDto>, GameResultsDataFetcher>();
builder.Services.AddScoped<IDocumentService<GameResultsDocumentDto>, DocumentService<GameResultsDocumentDto>>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler(app.Logger);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
