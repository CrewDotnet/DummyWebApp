using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PostgreSQL.Data;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace PostgreSQL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApiContext _context;

        public OrderRepository(ApiContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Games)!
                .ThenInclude(g => g.CompanyService)
                .Include(o => o.Customer)
                .ToListAsync();
            return orders;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Games)!
                .ThenInclude(g => g.CompanyService)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);
            return order;
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var orders = await _context.Orders
                .Include(o => o.Games)!
                .ThenInclude(g => g.CompanyService)
                .Include(o => o.Customer)
                .ToListAsync();
            return orders;
        }
    }
}
