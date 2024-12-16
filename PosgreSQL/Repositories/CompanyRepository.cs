using FluentResults;
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


        public async Task<Result<List<Company>>> GetAllAsync()
        {
            try
            {
                var getCompanies = await _context.Companies.Include(c => c.Games).ToListAsync();

                if (!getCompanies.Any())
                {
                    return Result.Fail(new Error("No companies found in database").WithMetadata("StatusCode", 404));
                }
                return Result.Ok(getCompanies);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }

        }

        public async Task<Result<Company>> GetByIdAsync(int id)
        {
            try
            {
                var getCompany =  await _context.Companies
                    .Include(c => c.Games)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (getCompany == null)
                {
                    return Result.Fail(new Error("Company not found").WithMetadata("StatusCode", 404));
                }

                return Result.Ok(getCompany);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }
        }

        public async Task<Result<Company>> AddAsync(Company request)
        {
            try
            {
                if (_context.Companies.Any(c => c.Name == request.Name))
                    return Result.Fail("Company with the same name already exists");

                request.Id = _context.Companies.Max(c => c.Id) + 1;
                _context.Companies.Add(request);
                await _context.SaveChangesAsync();

                var addedCompany = await _context.Companies
                    .Include(c => c.Games)
                    .FirstOrDefaultAsync(c => c.Id == request.Id);

                if (addedCompany == null)
                {
                    return Result.Fail("Failed to retrieve the newly added company");
                }

                return Result.Ok(addedCompany);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }
        }

        public async Task<Result> UpdateAsync(Company request)
        {
            try
            {
                var existingCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (existingCompany == null)
                {
                    return Result.Fail(new Error("Company does not exist")
                        .WithMetadata("StatusCode", 404));
                }
                _context.Companies.Update(request);
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

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var companyToDelete = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
                if (companyToDelete == null)
                    return Result.Fail($"Company with ID {id} does not exist.");

                _context.Companies.Remove(companyToDelete);
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
    }
}
