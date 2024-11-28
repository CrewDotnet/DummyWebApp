
using System.ComponentModel.DataAnnotations.Schema;

namespace PostgreSQL.DataModels
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public required Customer Customer { get; set; }
        public IEnumerable<Game>? Games { get; set; }
        public decimal SumOrder => Games?.Sum(game => game.Price) ?? 0;
        public Order()
        {
            
        }
        public Order(Customer customer, List<Game> games)
        {
            Customer = customer;
            Games = games ?? throw new ArgumentNullException(nameof(games));
        }
    }
}
