using DummyWebApp.Models.ResponseModels.Company;
using DummyWebApp.Models.ResponseModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Presenters.Order
{
    public class OrdersPresenter
    {
        public static ActionResult PresentOrders(List<OrderResponse> response)
        {
            return new OkObjectResult(response);
        }
    }
}
