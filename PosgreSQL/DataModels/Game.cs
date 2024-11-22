namespace PostgreSQL.DataModels
{
    public class Game
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public required Company Company { get; set; }
        public int? CompanyId { get; set; }
    }
}
