using Microsoft.EntityFrameworkCore;
using MovieRentalAPI.Common;
using MovieRentalAPI.Data;
using MovieRentalAPI.Models;

namespace MovieRentalAPI.Repositories
{
    public interface IBookStatusRepository
    {
        Task<CustomResult<List<BookStatus>>> GetAllAsync();
    }

    public class BookStatusRepository : IBookStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public BookStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResult<List<BookStatus>>> GetAllAsync()
        {
            try
            {
                var books = await _context.BookStatuses.ToListAsync();
                return new CustomResult<List<BookStatus>> { Success = true, Data = books };
            }
            catch (ArgumentNullException ex)
            {
                return new CustomResult<List<BookStatus>> { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
