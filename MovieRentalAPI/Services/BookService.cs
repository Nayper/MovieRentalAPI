using MovieRentalAPI.Common;
using MovieRentalAPI.Models;
using MovieRentalAPI.Repositories;
using System;

namespace MovieRentalAPI.Services
{
    public interface IBookService
    {
        Task<CustomResult<List<Book>>> GetAllAsync();
        Task<CustomResult<List<Book>>> GetSortedBooksAsync(string sortOrder, int pageNumber, int pageSize, bool isDesc);
        Task<CustomResult<Book>> AddBookAsync(Book book);
        Task<CustomResult<Book>> UpdateStatusAsync(int bookId, int newStatusId);
        Task<CustomResult<bool>> DeleteBookAsync(int bookId);
    }

    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private static readonly Dictionary<int, int[]> statusChangeMapping = new()
        {
            { 1, new int[] { 3, 4 }}, // Na półce, może zostać ustawiony tylko ze Zwrócona lub Uszkodzona
            { 2, new int[] { 1 } },   // Wypożyczona, może zostać ustawiony tylko z Na półce
            { 3, new int[] { 2 } },   // Zwrócona, może zostać ustawiony tylko z Wypożyczona
            { 4, new int[] { 1, 3} }  // Uszkodzona, może zostać ustawiony tylko z Na półce lub Zwrócona
        };

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<CustomResult<List<Book>>> GetAllAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<CustomResult<List<Book>>> GetSortedBooksAsync(string sortOrder, int pageNumber, int pageSize, bool isDesc)
        {
            return await _bookRepository.GetSortedBooksAsync(sortOrder, pageNumber, pageSize, isDesc);
        }

        public async Task<CustomResult<Book>> AddBookAsync(Book book)
        {
            return await _bookRepository.AddAsync(book);
        }

        public async Task<CustomResult<Book>> UpdateStatusAsync(int bookId, int newStatusId)
        {
            //Check if status exists
            if (!statusChangeMapping.ContainsKey(newStatusId))
            {
                return new CustomResult<Book> { Success = false, ErrorMessage = "Invalid book status ID!" };
            }

            // Retrieve the book to check its current status
            var getResult = await _bookRepository.GetByIdAsync(bookId);
            if (!getResult.Success)
            {
                return new CustomResult<Book> { Success = false, ErrorMessage = getResult.ErrorMessage };
            }

            var book = getResult.Data;

            // Check if the status change is valid
            if (book != null && !await CanChangeStatusAsync(book.BookStatusId, newStatusId))
            {
                return new CustomResult<Book>
                {
                    Success = false,
                    ErrorMessage = @"Cannot change status due to validation rules! 
                (1) Na półce może zostać ustawiona tylko ze Zwrócona lub Uszkodzona
                (2) Wypożyczona może zostać ustawiona tylko z Na półce
                (3) Zwrócona może zostać ustawiona tylko z Wypożyczona
                (4) Uszkodzona może zostać ustawiona tylko z Na półce lub Zwrócona"
                };
            }

            return await _bookRepository.UpdateStatusAsync(bookId, newStatusId);
        }

        private async Task<bool> CanChangeStatusAsync(int oldStatusId, int newStatusId)
        {
            return await Task.FromResult(statusChangeMapping.TryGetValue(newStatusId, out var allowedStatuses) &&
                                          allowedStatuses.Contains(oldStatusId));
        }

        public async Task<CustomResult<bool>> DeleteBookAsync(int bookId)
        {
            var result = await _bookRepository.GetByIdAsync(bookId);
            if (!result.Success)
            {
                return new CustomResult<bool> { Success = false, ErrorMessage = result.ErrorMessage };
            }

            return await _bookRepository.DeleteAsync(bookId);
        }
    }
}
