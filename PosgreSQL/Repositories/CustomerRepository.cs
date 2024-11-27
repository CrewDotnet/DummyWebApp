using Microsoft.EntityFrameworkCore;
using PostgreSQL.Data;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;


namespace PostgreSQL.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApiContext _context;

        public CustomerRepository(ApiContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var customers = await _context.Customers.Include(c => c.Games).ToListAsync();
            return customers;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            return customer;
        }

        public async Task<IEnumerable<Customer>> AddAsync(Customer newCustomer)
        {
            {
                if (_context.Customers.Any(c => c.FirstName == newCustomer.FirstName))
                    throw new ArgumentException("Customer with same name already exists");

                newCustomer.Id = _context.Customers.Max(c => c.Id) + 1;
                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();
                return _context.Customers.Include(c => c.Games).ToList();
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customerToDelete = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customerToDelete == null)
                return false;
            _context.Customers.Remove(customerToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(Customer updateCustomer)
        {
            _context.Customers.Update(updateCustomer);
            await _context.SaveChangesAsync();
        }
    }
}
