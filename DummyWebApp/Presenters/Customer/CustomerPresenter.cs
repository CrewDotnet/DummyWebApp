using DummyWebApp.Models.ResponseModels.Customer;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Presenters.Customer
{
    public class CustomerPresenter
    {
        public static ActionResult PresentCustomer(CustomerDTO response)
        {
            return new OkObjectResult(response);
        }
    }
}
