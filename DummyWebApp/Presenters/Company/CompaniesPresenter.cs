using DummyWebApp.Models.ResponseModels.Company;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Presenters.Company
{
    public class CompaniesPresenter
    {
        public static ActionResult PresentCompanies(CompaniesDTO response)
        {
            return new OkObjectResult(response);
        }

    }
}
