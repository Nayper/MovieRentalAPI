//using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieRentalAPI.Services;

namespace MovieRentalAPI.Controllers
{
    [ApiController]
    //[ApiVersion("1.0")]
    //[Route("api/v{version:apiVersion}")]
    [Route("api")]
    public class BookStatusesController : ControllerBase
    {
        private readonly IBookStatusService _bookStatusService;
        private readonly ILogger<BookStatusesController> _logger;

        public BookStatusesController(IBookStatusService bookStatusService, ILogger<BookStatusesController> logger)
        {
            _bookStatusService = bookStatusService;
            _logger = logger;
        }

        [HttpGet]
        [Route("bookstatuses")]
        public async Task<IActionResult> GetBookStatuses()
        {
            //_logger.LogInformation("Attempting to get statuses for book");
            var result = await _bookStatusService.GetAllAsync();
            if (!result.Success)
            {
                //_logger.LogWarning("Book statuses not found");
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}