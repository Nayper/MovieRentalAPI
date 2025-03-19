using Microsoft.EntityFrameworkCore;
using MovieRentalAPI.Common;
using MovieRentalAPI.Data;
using MovieRentalAPI.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace MovieRentalAPI.Repositories
{
    public interface IBookRepository
    {
        Task<CustomResult<List<Book>>> GetAllAsync();
        Task<CustomResult<List<Book>>> GetSortedBooksAsync(string sortOrder, int pageNumber, int pageSize, bool isDesc);
        Task<CustomResult<Book>> GetByIdAsync(int id);
        Task<CustomResult<Book>> AddAsync(Book book);
        Task<CustomResult<Book>> UpdateStatusAsync(int bookId, int newStatusId);
        Task<CustomResult<bool>> DeleteAsync(int id);
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

        public async Task<CustomResult<List<Book>>> GetSortedBooksAsync(string sortOrder, int pageNumber, int pageSize, bool isDesc)
        {
            try
            {
                var booksQuery = _context.Books.AsQueryable();

                // Sortowanie
                switch (sortOrder)
                {
                    case "id":
                        booksQuery = isDesc ? booksQuery.OrderByDescending(b => b.Id) : booksQuery.OrderBy(b => b.Id);
                        break;
                    case "title":
                        booksQuery = isDesc ? booksQuery.OrderByDescending(b => b.Title) : booksQuery.OrderBy(b => b.Title);
                        break;
                    case "author":
                        booksQuery = isDesc ? booksQuery.OrderByDescending(b => b.Author) : booksQuery.OrderBy(b => b.Author);
                        break;
                    case "ISBN":
                        booksQuery = isDesc ? booksQuery.OrderByDescending(b => b.ISBN) : booksQuery.OrderBy(b => b.ISBN);
                        break;
                    default:
                        booksQuery = isDesc ? booksQuery.OrderByDescending(b => b.Title) : booksQuery.OrderBy(b => b.Title);
                        break;
                }

                var pagedBooks = await booksQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                .ToListAsync();

                return new CustomResult<List<Book>> { Success = true, Data = pagedBooks };
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
        public async Task<CustomResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return new CustomResult<bool> { Success = false, ErrorMessage = "Failed to delete the book." };
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return new CustomResult<bool>
                {
                    Success = true,
                    Data = true
                };
            }
            catch
            {
                return new CustomResult<bool> { Success = false, ErrorMessage = "Failed to delete the book." };
            }
        }
    }
}
