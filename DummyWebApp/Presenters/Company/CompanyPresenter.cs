using DummyWebApp.Models.ResponseModels.Company;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Presenters.Company
{
    public class CompanyPresenter
    {
        public static ActionResult PresentCompany(CompanyDTO response)
        {
            return new OkObjectResult(response);
        }
    }
}
