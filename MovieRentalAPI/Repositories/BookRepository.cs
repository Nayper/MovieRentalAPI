using Microsoft.EntityFrameworkCore;
using MovieRentalAPI.Common;
using MovieRentalAPI.Data;
using MovieRentalAPI.Models;

namespace MovieRentalAPI.Repositories
{
    public interface IBookRepository
    {
        Task<CustomResult<List<Book>>> GetAllAsync();
        Task<CustomResult<Book>> GetByIdAsync(int id);
        Task<CustomResult<Book>> AddAsync(Book book);
        Task<CustomResult<Book>> UpdateStatusAsync(int bookId, int newStatusId);
    }

    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResult<List<Book>>> GetAllAsync()
        {
            try
            {
                var books = await _context.Books.ToListAsync();
                return new CustomResult<List<Book>> { Success = true, Data = books };
            }
            catch (ArgumentNullException ex)
            {
                return new CustomResult<List<Book>> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<CustomResult<Book>> GetByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return new CustomResult<Book> { Success = false, ErrorMessage = $"Book with ID {id} not found." };
            }
            return new CustomResult<Book> { Success = true, Data = book };
        }

        public async Task<CustomResult<Book>> AddAsync(Book book)
        {
            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return new CustomResult<Book> { Success = true, Data = book };
            }
            catch (Exception ex)
            {
                return new CustomResult<Book> { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<CustomResult<Book>> UpdateStatusAsync(int bookId, int newStatusId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return new CustomResult<Book>
                {
                    Success = false,
                    ErrorMessage = "Book not found."
                };
            }

            book.BookStatusId = newStatusId;

            try
            {
                await _context.SaveChangesAsync();
                return new CustomResult<Book>
                {
                    Success = true,
                    Data = book
                };
            }
            catch (DbUpdateException ex)
            {
                return new CustomResult<Book>
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
