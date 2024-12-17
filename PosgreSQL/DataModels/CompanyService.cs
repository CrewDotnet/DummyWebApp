namespace PostgreSQL.DataModels
{
    public class CompanyService
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Game>? Games { get; set; }
    }
}
