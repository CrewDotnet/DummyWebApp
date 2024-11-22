using PostgreSQL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreSQL.Data;
using PostgreSQL.DataModels;

namespace PostgreSQL.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApiContext _context;

        public CompanyRepository(ApiContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            var companies = await _context.Companies.Include(c => c.Games).ToListAsync();
            return companies;
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _context.Companies.Include(c => c.Games).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Company>> AddAsync(Company newCompany)
        {
            if (_context.Companies.Any(c => c.Name == newCompany.Name))
                throw new ArgumentException("Game with same name already exists");

            newCompany.Id = _context.Companies.Max(c => c.Id) + 1;
            _context.Companies.Add(newCompany);
            await _context.SaveChangesAsync();
            return _context.Companies.Include(c => c.Games).ToList();
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var companyToDelete = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (companyToDelete == null)
                return false;
            _context.Companies.Remove(companyToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
