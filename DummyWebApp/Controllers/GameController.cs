using AutoMapper;
using DummyWebApp.Models.ErrorModel;
using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Models.ResponseModels.Company;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Presenters.Company;
using DummyWebApp.Presenters.Erorr;
using DummyWebApp.Presenters.Game;
using DummyWebApp.Services;
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
        private readonly IMapper _mapper;

        public GameController(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;
        }
        // GET: api/Game
        [HttpGet("~/api/Games")]
        [ProducesResponseType(typeof(GameDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<List<GameDTO>>> Get()
        {
            var result = await _gameService.GetAllGames();
            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<List<GameDTO>>(result.Value);
                return GamesPresenter.PresentGames(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // GET api/Game/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GameDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<GameDTO>> Get(int id)
        {
            var result = await _gameService.GetGameById(id);
            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<GameDTO>(result.Value);
                return GamePresenter.PresentGame(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // POST api/Game
        [HttpPost]
        [ProducesResponseType(typeof(GameDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<GameDTO>> Post([FromBody] NewGameRequest request)
        {
            var result = await _gameService.AddGame(request);

            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<GameDTO>(result.Value);
                return GamePresenter.PresentGame(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // PUT api/Game/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GameDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<GameDTO?>> Put(int id, [FromBody] UpdateGameRequest request)
        {
            var result = await _gameService.UpdateGame(id, request);
            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<GameDTO>(result.Value);
                return GamePresenter.PresentGame(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // DELETE api/Game/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _gameService.DeleteGame(id);
            if (result.IsSuccess)
            {
                return true;
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }
    }
}
