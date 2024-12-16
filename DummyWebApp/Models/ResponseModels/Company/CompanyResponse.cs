using DummyWebApp.Models.ResponseModels.Game;

namespace DummyWebApp.Models.ResponseModels.Company
{
    public class CompanyResponse
    {
        public required string Name { get; set; }
        public List<GameBaseResponse>? Games { get; set; }
    }
}
