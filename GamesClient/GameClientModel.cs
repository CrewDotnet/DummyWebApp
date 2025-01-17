namespace GamesClient
{
    public class GameClientModel
    {
        //[Newtonsoft.Json.JsonProperty("title", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Id { get; set; }
        public string Title { get; set; }
        [Newtonsoft.Json.JsonProperty("short_description", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]

        public string ShortDescription { get; set; }
        public string Genre { get; set; }
        public string Platform { get; set; }
        public string Publisher { get; set; }
        [Newtonsoft.Json.JsonProperty("release_date", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ReleaseDate { get; set; }
    }
}
