namespace PostgreSQL.DataModels
{
    public class Company
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Game>? Games { get; set; }
    }
}
