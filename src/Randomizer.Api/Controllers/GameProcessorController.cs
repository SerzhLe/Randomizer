using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Randomizer.Api.Extensions;
using Randomizer.Application.DTOs;
using Randomizer.Application.DTOs.FileSystem;
using Randomizer.Application.Services;

namespace Randomizer.Api.Controllers;

public class GameProcessorController : BaseController
{
    private readonly IGameProcessorService _gameProcessorService;
    private readonly IDocumentService<GameResultsDocumentDto> _documentService;

    public GameProcessorController(
        IGameProcessorService gameProcessorService, 
        IDocumentService<GameResultsDocumentDto> documentService)
    {
        _gameProcessorService = gameProcessorService;
        _documentService = documentService;
    }

    [HttpPost("startGame")]
    public async Task<ActionResult<GameConfigDto>> StartGame(CreateGameConfigDto createGameConfigDto)
    {
        var result = await _gameProcessorService.StartGame(createGameConfigDto);

        return result.ToActionResult();
    }

    [HttpPost("game/{gameConfigId:guid}/startNewRound")]
    public async Task<ActionResult<RoundDto>> StartNewRound(Guid gameConfigId)
    {
        var result = await _gameProcessorService.StartNewRound(gameConfigId);

        return result.ToActionResult();
    }

    [HttpGet("game/{gameConfigId:guid}/randomData")]
    public async Task<ActionResult<RoundResultDto>> GetRandomData(Guid gameConfigId)
    {
        var result = await _gameProcessorService.GetRandomData(gameConfigId);

        return result.ToActionResult();
    }

    [HttpPut("game/updateRoundResult")]
    public async Task<ActionResult<RoundResultDto>> UpdateRoundResultWithFeedback(UpdateRoundResultDto updateRoundResultDto)
    {
        var result = await _gameProcessorService.UpdateRoundResultWithFeedback(updateRoundResultDto);

        return result.ToActionResult();
    }

    [HttpGet("game/{gameConfigId:guid}/gameResults")]
    public async Task<ActionResult<RoundResultDto>> GetGameResults(Guid gameConfigId)
    {
        var result = await _gameProcessorService.GetGameResults(gameConfigId);

        return result.ToActionResult();
    }

    [HttpGet("game/{gameConfigId:guid}/pdf")]
    public async Task<ActionResult> GetGameResultsPdfSummary(Guid gameConfigId)
    {
        var result = await _documentService.GenerateDocumentAsync(gameConfigId);

        if (!result.IsSuccessful)
        {
            return result.ToActionResult();
        }

        Response.Headers.ContentDisposition = new StringValues($"attachment; filename={gameConfigId}.pdf");

        return File(result.Payload!, "application/pdf");
    }
}
