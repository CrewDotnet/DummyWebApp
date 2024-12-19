using DummyWebApp.Models.ResponseModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Presenters.Order
{
    public class OrderPresenter
    {
        public static ActionResult PresentOrder(OrderResponse response)
        {
            return new OkObjectResult(response);
        }
    }
}
