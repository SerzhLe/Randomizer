using Microsoft.AspNetCore.Mvc;
using Randomizer.Api.Extensions;
using Randomizer.Core.DTOs;
using Randomizer.Core.Services;

namespace Randomizer.Api.Controllers;
public class GameProcessorController : BaseController
{
    private readonly IGameProcessorService _gameProcessorService;

    public GameProcessorController(IGameProcessorService gameProcessorService)
    {
        _gameProcessorService = gameProcessorService;
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
}
