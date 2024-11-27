using DummyWebApp.ResponseModels;
using Microsoft.EntityFrameworkCore;
using PostgreSQL.DataModels;

namespace DummyWebApp.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<Order>> GetAllOrdersAsync();
        public Task<OrderResponse> PlaceOrderAsync(int customerId, IEnumerable<int> gameIds);
        public Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);

    }
}
