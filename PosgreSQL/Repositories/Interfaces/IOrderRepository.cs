using PostgreSQL.DataModels;

namespace PostgreSQL.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task CreateOrderAsync(Order order);
        Task DeleteAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
    }
}
