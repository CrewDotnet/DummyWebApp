using FluentResults;
using Microsoft.EntityFrameworkCore;
using PostgreSQL.Data;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PostgreSQL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApiContext _context;
        public GameRepository(ApiContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Game>>> GetAllAsync()
        {
            var games = await _context.Games
                .Include(g => g.Company)
                .ToListAsync();
            try
            {
                if (!games.Any())
                {
                    return Result.Fail(
                        new Error("There is no game record in database").WithMetadata("StatusCode", 404));
                }

                return Result.Ok(games);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }
        }

        public async Task<Result<Game>> GetByIdAsync(int id)
        {
            var game =  await _context.Games
                .Include(g => g.Company)
                .FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                if (game == null)
                {
                    return Result.Fail(new Error($"No game with ID {id} ").WithMetadata("StatusCode", 404));
                }
                return Result.Ok(game);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }
        }

        public async Task<Result<Game>> AddAsync(Game request)
        {
            try
            {
                if (_context.Games.Any(g => g.Name == request.Name))
                    return Result.Fail(
                        new Error("Game with provided name already exists").WithMetadata("StatusCode", 400));

                request.Id = _context.Games.Max(g => g.Id) + 1;
                _context.Games.Add(request);
                await _context.SaveChangesAsync();

                return Result.Ok(request);
            }
            catch (Exception e)
            {
                return Result.Fail(new Error(e.Message).CausedBy(e)
                    .WithMetadata("StatusCode", 500)
                    .WithMetadata("ExceptionMessage", e.Message)
                    .WithMetadata("StackTrace", e.StackTrace));
            }
        }

        public async Task<Result> UpdateAsync(Game request)
        {
            try
            {
                var existingGame = await _context.Companies.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (existingGame == null)
                {
                    return Result.Fail(new Error("Game does not exist")
                        .WithMetadata("StatusCode", 404));
                }
                _context.Games.Update(request);
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
                var gameToDelete = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
                if (gameToDelete == null)
                    return Result.Fail($"Game with ID {id} does not exist.");

                _context.Games.Remove(gameToDelete);
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

        public async Task<Result<List<Game>>> GetGameCollectionByIds(IEnumerable<int> ids)
        {
            try
            {
                var games = await _context.Games
                    .Include(g => g.Company)
                    .Where(g => ids.Contains(g.Id)).ToListAsync();
                if (!games.Any())
                {
                    return Result.Fail(new Error("No game/games with provided ids").WithMetadata("StatusCode", 404));
                }
                return Result.Ok(games);
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
