namespace PostgreSQL.DataModels
{
    public class Game
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required decimal Price { get; set; }
        public Company? Company { get; set; }
        public required int CompanyId { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public string ShortDescription { get; set; }
        public string Genre { get; set; }
        public string Platform { get; set; }
        public string ReleaseDate { get; set; }
    }
}
