using PostgreSQL.DataModels;

namespace DummyWebApp.ResponseModels
{
    public class CustomerResponse
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public int LoyaltyPoints { get; set; } = 0;
        public List<Game>? Games { get; set; }
    }
}
