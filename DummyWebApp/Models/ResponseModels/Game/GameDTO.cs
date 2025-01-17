namespace DummyWebApp.Models.ResponseModels.Game
{
    public class GameDTO : GameBaseResponse
    {
        public string? Company { get; set; }
        public IEnumerable<int> OrderIds { get; set; } = [];
        public string ShortDescription { get; set; }
        public string Genre { get; set; }
        public string Platform { get; set; }
        public string ReleaseDate { get; set; }
    }
}
