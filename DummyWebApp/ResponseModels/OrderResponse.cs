using PostgreSQL.DataModels;

namespace DummyWebApp.ResponseModels
{
    public class OrderResponse2
    {
        public OrderResponse Order { get; set; }
    }
    public class OrderResponse
    {
        public int Id { get; set; }
        public required string CustomerFullName { get; set; }
        public required IEnumerable<GameResponseWithCompany> Games { get; set; }
        public decimal SumOrder { get; set; }
    }
}
