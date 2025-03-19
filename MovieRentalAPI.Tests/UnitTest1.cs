
using global::MovieRentalAPI.Common;
using global::MovieRentalAPI.Models;
using global::MovieRentalAPI.Repositories;
using global::MovieRentalAPI.Services;
using Moq;

namespace MovieRentalAPI.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _bookService = new BookService(_mockBookRepository.Object);
        }

        [Fact]
        public async Task UpdateStatusAsync_InvalidStatusId_ReturnsError()
        {
            // Arrange
            int bookId = 1;
            int invalidStatusId = 99; // Nieprawidłowy status

            // Act
            var result = await _bookService.UpdateStatusAsync(bookId, invalidStatusId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid book status ID!", result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateStatusAsync_BookNotFound_ReturnsError()
        {
            // Arrange
            int bookId = 1;
            int newStatusId = 2; // Wypożyczona

            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
                .ReturnsAsync(new CustomResult<Book> { Success = false, ErrorMessage = "Book not found." });

            // Act
            var result = await _bookService.UpdateStatusAsync(bookId, newStatusId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Book not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateStatusAsync_ValidStatusChange_ReturnsSuccess()
        {
            // Arrange
            int bookId = 1;
            int statusId = 2; // Wypożyczona
            int newStatusId = 3; // Zwrócona
            var book = new Book { Id = bookId, Title= "Hobbit", Author= "Tolkien", ISBN= "123", BookStatusId = statusId };

            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
                .ReturnsAsync(new CustomResult<Book> { Success = true, Data = book });

            _mockBookRepository.Setup(repo => repo.UpdateStatusAsync(bookId, newStatusId))
                .ReturnsAsync(new CustomResult<Book> { Success = true, Data = book });

            // Act
            var result = await _bookService.UpdateStatusAsync(bookId, newStatusId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(book, result.Data);
        }

        [Fact]
        public async Task UpdateStatusAsync_InvalidStatusChange_ReturnsError()
        {
            // Arrange
            int bookId = 1;
            int newStatusId = 4; // Uszkodzona
            var book = new Book { Id = bookId, Title = "Hobbit", Author = "Tolkien", ISBN = "123", BookStatusId = 2 }; // Wypożyczona

            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
                .ReturnsAsync(new CustomResult<Book> { Success = true, Data = book });

            // Act
            var result = await _bookService.UpdateStatusAsync(bookId, newStatusId);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Cannot change status due to validation rules!", result.ErrorMessage);
        }
    }
}