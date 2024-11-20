using DummyWebApp.Data;
using DummyWebApp.Models;
using DummyWebApp.Services.Interfaces;
using DummyWebApp.Services.Interfaces.Game;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DummyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService, ApiContext context)
        {
            _gameService = gameService;
        }
        // GET: api/Game
        [HttpGet]
        public async Task<ActionResult<List<GameDTO>>> Get()
        {
            var allGames = await _gameService.GetAllGames();
            if (!allGames.Any())
                return NotFound("There is no game in the list");
            return Ok(allGames);
        }

        // GET api/Game/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDTO>> Get(int id)
        {
            var game = await _gameService.GetGameById(id);
            if (game == null)
                return NotFound($"The game with ID:{id} doesn't exists");
            return Ok(game);
        }

        // POST api/Game
        [HttpPost]
        public async Task<ActionResult<GameDTO>> Post([FromBody] Game? newGame)
        {
            if (newGame == null)
                return BadRequest("Bad request");
            if (string.IsNullOrWhiteSpace((newGame.Name)))
                return BadRequest("Name required");
            return Ok(await _gameService.AddGame(newGame));
        }

        // PUT api/Game/5
        [HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] Game update)
        //{
        //    bool gameUpdated = _gameService.UpdateGame(id,update);
        //    if (!gameUpdated)
        //    {
        //        return BadRequest("Wrong request");
        //    }

        //    return Ok("Game has been successfully updated");
        //}

        // DELETE api/Game/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var isDeleted = await _gameService.DeleteGame(id);
            if (!isDeleted)
            {
                return BadRequest("Game doesn't exist");
            }

            return Ok("Game has been successfully deleted");
        }
    }
}
