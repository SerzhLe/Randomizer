using FluentValidation;
using Randomizer.Api.Extensions;
using Randomizer.Application;
using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Application.Services;
using Randomizer.Application.Services.DocumentDataFetchers;
using Randomizer.Application.Validation;
using Randomizer.Infrastructure;
using Randomizer.Persistence;

var builder = WebApplication.CreateBuilder(args);

// global configs
ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
ValidatorOptions.Global.LanguageManager = new ValidationLanguageManager
{
    Enabled = false,
};

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddAutoMapper(ApplicationAssemblyReference.Reference);
builder.Services.AddScoped<ICoreValidator, FluentCoreValidator>();

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
