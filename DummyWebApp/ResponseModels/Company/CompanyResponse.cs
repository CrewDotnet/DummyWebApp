using DummyWebApp.ResponseModels.Game;

namespace DummyWebApp.ResponseModels.Company
{
    public class CompanyResponse
    {
        public required string Name { get; set; }
        public List<GameBaseResponse>? Games { get; set; }
    }
}
