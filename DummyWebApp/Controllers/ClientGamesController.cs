using DummyWebApp.Models.ErrorModel;
using DummyWebApp.Models.ResponseModels.Company;
using DummyWebApp.Presenters.Company;
using DummyWebApp.Presenters.Erorr;
using DummyWebApp.Presenters.Game;
using DummyWebApp.Services.Interfaces;
using GamesClient;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DummyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientGamesController : ControllerBase
    {
        private readonly IGameClientService _gameClientService;

        public ClientGamesController(IGameClientService gameClientService)
        {
            _gameClientService = gameClientService;
        }

        // POST api/<ClientGamesController>
        /// <summary>
        /// Imports or updates (existing games) from client API for the existing companies.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(GameClientModel), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<bool>> Post()
        {
            var result = await _gameClientService.AddOrUpdateClientGamesToExistingCompany();
            if (result.IsSuccess)
            {
                return ClientGamePresenter.PresentClientGame(result.Value);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }
    }
}
