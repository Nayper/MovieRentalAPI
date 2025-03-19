using MovieRentalAPI.Common;
using MovieRentalAPI.Models;
using MovieRentalAPI.Repositories;

namespace MovieRentalAPI.Services
{
    public interface IBookStatusService
    {
        Task<CustomResult<List<BookStatus>>> GetAllAsync();
    }

    public class BookStatusService : IBookStatusService
    {
        private readonly IBookStatusRepository _bookStatusRepository;

        public BookStatusService(IBookStatusRepository bookStatusRepository)
        {
            _bookStatusRepository = bookStatusRepository;
        }

        public async Task<CustomResult<List<BookStatus>>> GetAllAsync()
        {
            return await _bookStatusRepository.GetAllAsync();
        }
    }
}