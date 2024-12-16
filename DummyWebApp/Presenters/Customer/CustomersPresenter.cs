using DummyWebApp.Models.ResponseModels.Company;
using DummyWebApp.Models.ResponseModels.Customer;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Presenters.Customer
{
    public class CustomersPresenter
    {
        public static ActionResult PresentCustomers(CustomersDTO response)
        {
            return new OkObjectResult(response);
        }
    }
}
