namespace PostgreSQL.DataModels
{
    public class Game
    {
        public int Id { get; set; }
        public required string? Name { get; set; }
        public required decimal Price { get; set; }
        public required CompanyService CompanyService { get; set; }
        public int? CompanyId { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
