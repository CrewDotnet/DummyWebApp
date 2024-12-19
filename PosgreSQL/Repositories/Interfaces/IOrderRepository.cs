using FluentResults;
using PostgreSQL.DataModels;

namespace PostgreSQL.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Result<List<Order>>> GetAllAsync();
        Task<Result<Order>> GetByIdAsync(int id);
        Task<Result<Order>> CreateOrderAsync(Order order);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<List<Order>>> GetOrdersByCustomerIdAsync(int customerId);
    }
}
