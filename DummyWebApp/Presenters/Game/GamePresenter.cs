using DummyWebApp.Models.ResponseModels.Game;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Presenters.Game
{
    public class GamePresenter
    {
        public static ActionResult PresentGame(GameDTO response)
        {
            return new OkObjectResult(response);
        }
    }
}
