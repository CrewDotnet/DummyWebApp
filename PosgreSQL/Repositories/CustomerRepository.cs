using FluentResults;
using Microsoft.EntityFrameworkCore;
using PostgreSQL.Data;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace PostgreSQL.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApiContext _context;

        public CustomerRepository(ApiContext context)
        {
            _context = context;
        }
        public async Task<Result<List<Customer>>> GetAllAsync()
        {
            try
            {
                var customers = await _context.Customers
                    .Include(c => c.Games)!
                    .ToListAsync();
                if (!customers.Any())
                {
                    return Result.Fail(new Error("No customer record in database").WithMetadata("StatusCode", 404));
                }
                return Result.Ok(customers);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }

        }

        public async Task<Result<Customer>> GetByIdAsync(int id)
        {
            try
            {
                var customer = await _context.Customers
                    .Include(c => c.Games)!
                    .ThenInclude(g => g.Company)
                    .Include(c => c.Games)!
                    .ThenInclude(g => g.Orders)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (customer == null)
                {
                    return Result.Fail(new Error("Customer not found").WithMetadata("StatusCode", 404));
                }
                return Result.Ok(customer);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }
        }

        public async Task<Result<List<Customer>>> AddAsync(Customer request)
        {
            try
            {
                {
                    if (_context.Customers.Any(c => c.EmailAddress == request.EmailAddress))
                        return Result.Fail(
                            new Error("Customer with same email already exists").WithMetadata("StatusCode", 400));

                    request.Id = _context.Customers.Max(c => c.Id) + 1;
                    _context.Customers.Add(request);
                    await _context.SaveChangesAsync();

                    var response = _context.Customers.Include(c => c.Games).ToList();
                    return Result.Ok(response);
                }
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
                var customerToDelete = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customerToDelete == null)
                    return Result.Fail(new Error("Provided customer does not exist").WithMetadata("StatusCode", 404));

                _context.Customers.Remove(customerToDelete);

                if (await _context.Customers.AnyAsync(c => c.Id == customerToDelete.Id))
                {
                    await _context.SaveChangesAsync();
                }

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

        public async Task<Result> UpdateAsync(Customer request)
        {
            try
            {
                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (existingCustomer == null)
                {
                    return Result.Fail(new Error("Company does not exist")
                        .WithMetadata("StatusCode", 404));
                }
                _context.Customers.Update(request);
                await _context.SaveChangesAsync();

                return Result.Ok();
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
    }
}
