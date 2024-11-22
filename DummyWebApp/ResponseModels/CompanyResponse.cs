using PostgreSQL.DataModels;

namespace DummyWebApp.ResponseModels
{
    public class CompanyResponse
    {
        public required string Name { get; set; }
        public List<GameResponseForCompany>? Games { get; set; }
    }
}
