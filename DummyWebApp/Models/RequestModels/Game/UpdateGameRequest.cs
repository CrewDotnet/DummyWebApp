namespace DummyWebApp.Models.RequestModels.Game
{
    public class UpdateGameRequest
    {
        public required string? Title { get; set; }
        public required decimal Price { get; set; }
    }
}
