namespace DummyWebApp.Models.ResponseModels.Game
{
    public class GameDTO : GameBaseResponse
    {
        public string? Company { get; set; }
        public IEnumerable<int> OrderIds { get; set; } = [];
    }
}
