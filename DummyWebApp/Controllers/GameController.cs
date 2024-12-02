using DummyWebApp.RequestModels.Game;
using DummyWebApp.ResponseModels.Game;
using DummyWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PostgreSQL.Data;
using PostgreSQL.DataModels;

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
        public async Task<ActionResult<List<GameResponseWithCompany>>> Get()
        {
            var allGames = await _gameService.GetAllGames();
            if (!allGames.Any())
                return NotFound("There is no game in the list");
            return Ok(allGames);
        }

        // GET api/Game/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameResponseWithCompany>> Get(int id)
        {
            var game = await _gameService.GetGameById(id);
            if (game == null)
                return NotFound($"The game with ID:{id} doesn't exists");
            return Ok(game);
        }

        // POST api/Game
        [HttpPost]
        public async Task<ActionResult<GameResponseWithCompany>> Post([FromBody] NewGameRequest? newGame)
        {
            if (newGame == null)
                return BadRequest("Bad request");
            if (string.IsNullOrWhiteSpace((newGame.Name)))
                return BadRequest("Name required");
            return Ok(await _gameService.AddGame(newGame));
        }

        // PUT api/Game/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GameResponseWithCompany?>> Put(int id, [FromBody] UpdateGameRequest update)
        {
            var gameUpdated = await _gameService.UpdateGame(id, update);
            if (gameUpdated!.Name == string.Empty || gameUpdated.Price <= 0)
            {
                return BadRequest("Wrong request");
            }

            return Ok("Game has been successfully updated");
        }

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
