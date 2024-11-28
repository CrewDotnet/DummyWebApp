namespace PostgreSQL.DataModels
{
    public class Customer
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public int LoyaltyPoints { get; set; } = 0;
        public decimal TotalAmountSpent { get; set; }
        public List<Game>? Games { get; set; } = [];
        public IEnumerable<Order> Orders { get; set; } = new List<Order>();
    }
}
