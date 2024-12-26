using PostgreSQL.DataModels;
namespace DummyWebApp.Models.RequestModels.Game
{
    public class NewGameRequest
    {
        public required string? Title { get; set; }
        public required decimal Price { get; set; }
        public required int CompanyId { get; set; }
    }
}
