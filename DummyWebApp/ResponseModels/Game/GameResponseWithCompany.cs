namespace DummyWebApp.ResponseModels.Game
{
    public class GameResponseWithCompany : GameBaseResponse
    {
            public string? Company { get; set; }
            public IEnumerable<int> OrderIds { get; set; } = [];
    }
}
