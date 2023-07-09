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
    public async Task<ActionResult> StartGame(CreateGameConfigDto createGameConfigDto)
    {
        var result = await _gameProcessorService.StartGame(createGameConfigDto);

        return result.ToActionResult();
    }
}
