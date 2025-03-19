//using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieRentalAPI.Services;

namespace MovieRentalAPI.Controllers
{
    [ApiController]
    //[ApiVersion("1.0")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        [Route("books")]
        public async Task<IActionResult> GetAllBooks()
        {
            var result = await _bookService.GetAllAsync();
            if (!result.Success)
            {
                _logger.LogWarning("Books not found");
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{id}/updatestatus")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] int newStatusId)
        {
            var result = await _bookService.UpdateStatusAsync(id, newStatusId);

            if (!result.Success)
            {
                _logger.LogWarning($"Could not update book ID {id}");
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}
