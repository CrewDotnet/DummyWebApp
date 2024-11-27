using PostgreSQL.DataModels;

namespace DummyWebApp.ResponseModels
{
    public class GameResponseWithCompany
    {
            public int Id { get; set; }
            public string? Name { get; set; }
            public decimal Price { get; set; }
            public string? Company { get; set; }
    }
}
