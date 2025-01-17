using DummyWebApp.Models.ResponseModels.Game;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Presenters.Game
{
    public class ClientGamePresenter
    {
        public static ActionResult PresentClientGame(bool response)
        {
            return new OkObjectResult(response);
        }

    }
}
