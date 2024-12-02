namespace DummyWebApp.RequestModels.Game
{
    public class UpdateGameRequest
    {
        public required string? Name { get; set; }
        public required decimal Price { get; set; }
    }
}
