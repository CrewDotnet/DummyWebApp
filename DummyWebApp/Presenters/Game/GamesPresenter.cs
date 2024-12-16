using DummyWebApp.Models.ResponseModels.Game;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Presenters.Game
{
    public class GamesPresenter
    {
        public static ActionResult PresentGames(List<GameDTO> response)
        {
            return new OkObjectResult(response);
        }
    }
}
