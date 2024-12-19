using FluentResults;
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
        public async Task<Result<List<Order>>> GetAllAsync()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Games)!
                    .ThenInclude(g => g.Company)
                    .Include(o => o.Customer)
                    .ToListAsync();
                if (!orders.Any())
                {
                    Result.Fail(new Error("There is no records of orders in database").WithMetadata("StatusCode", 404));
                }
                return Result.Ok(orders);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }
        }

        public async Task<Result<Order>> GetByIdAsync(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.Games)!
                    .ThenInclude(g => g.Company)
                    .Include(o => o.Customer)
                    .FirstOrDefaultAsync(o => o.Id == id);
                if (order == null)
                {
                    return Result.Fail(new Error("There is no order with provided id").WithMetadata("StatusCode", 404));
                }

                return Result.Ok(order);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }
        }


        public async Task<Result<Order>> CreateOrderAsync(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                return Result.Ok(order);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var orderToDelete = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (orderToDelete == null)
                    return Result.Fail($"Order with ID {id} does not exist.");

                _context.Orders.Remove(orderToDelete);
                await _context.SaveChangesAsync();

                return Result.Ok(true);
            }
            catch (DbUpdateException e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }

        }

        public async Task<Result<List<Order>>> GetOrdersByCustomerIdAsync(int customerId)
        {
            try
            {
                var orders = await _context.Orders.Where(o => o.CustomerId == customerId)
                    .Include(o => o.Games)!
                    .ThenInclude(g => g.Company)
                    .Include(o => o.Customer)
                    .ToListAsync();
                if (!orders.Any())
                {
                    return Result.Fail(
                        new Error($"There is no order with customer id {customerId}").WithMetadata("StatusCode", 404));
                }
                return Result.Ok(orders);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }
        }
    }
}
