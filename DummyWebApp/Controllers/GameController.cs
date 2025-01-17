using AutoMapper;
using DummyWebApp.MediatR.Commands;
using DummyWebApp.MediatR.Queries;
using DummyWebApp.Models.ErrorModel;
using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Presenters.Erorr;
using DummyWebApp.Presenters.Game;
using DummyWebApp.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GameController(IGameService gameService, IMapper mapper, IMediator mediator)
        {
            _gameService = gameService;
            _mapper = mapper;
            _mediator = mediator;
        }
        // GET: api/Game
        [HttpGet("~/api/Games")]
        [ProducesResponseType(typeof(GameDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<List<GameDTO>>> Get()
        {
            var result = await _mediator.Send(new GetGameListQuery());

            //var result = await _gameService.GetAllGames();
            if (result.IsSuccess)
            {
                return GamesPresenter.PresentGames(result.Value);
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
            var result = await _mediator.Send(new GetGameByIdQuery(id));

            //var result = await _gameService.GetGameById(id);
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
        public async Task<ActionResult<GameBaseResponse>> Post([FromBody] NewGameRequest request)
        {
            var result = _mediator.Send(new AddGameCommand(request.Title, request.Price, request.CompanyId));
            //var result = await _gameService.AddGame(request);

            if (result.Result.IsSuccess)
            {
                var mappedResult = _mapper.Map<GameDTO>(result.Result.Value);
                return GamePresenter.PresentGame(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Result.Errors);
        }

        // PUT api/Game/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GameDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<GameDTO?>> Put(int id, [FromBody] UpdateGameRequest request)
        {
            //var result = await _gameService.UpdateGame(id, request);
            var result = await _mediator.Send(new UpdateGameCommand(id, request.Genre, request.ShortDescription, request.Platform));
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
            //var result = await _gameService.DeleteGame(id);
            var result = await _mediator.Send(new DeleteGameCommand(id));
            if (result.IsSuccess)
            {
                return true;
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }
    }
}
