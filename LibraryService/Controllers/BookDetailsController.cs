using Library.Common.Models;
using Library.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace Library.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookDetailsController : ControllerBase
    {
        private readonly ILogger<BookDetailsController> _logger;
        private readonly IBookRepository _bookRepository;

        public BookDetailsController(ILogger<BookDetailsController> logger, IBookRepository bookRepository)
        {
            _logger = logger;
            _bookRepository = bookRepository;
        }

        /// <summary>
        /// Get all the Books
        /// </summary>
        /// <returns></returns>
        [Route("FetchAllBooks")]
        [SwaggerOperation(OperationId = "FetchAllBooks")]
        [HttpGet]
        public async Task<IActionResult> FetchAllBooks()
        {
            try
            {
                var result = await _bookRepository.GetAllBooksAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    Content = ex.Message,
                    StatusCode = 500
                };
            }
        }

        /// <summary>
        /// Gets a list of books with matching Book Name or Author
        /// </summary>
        /// <returns></returns>
        [Route("SearchBooks")]
        [SwaggerOperation(OperationId = "SearchBooks")]
        [HttpGet]
        public async Task<IActionResult> SearchBooks([FromQuery]string query)
        {
            try
            {
                var result = await _bookRepository.SearchBooks(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    Content = ex.Message,
                    StatusCode = 500
                };
            }
        }

        /// <summary>
        /// Adds a single book
        /// </summary>
        /// <returns></returns>
        [Route("AddBook")]
        [SwaggerOperation(OperationId = "AddBook")]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody]AddBooksInput book)
        {
            try
            {
                await _bookRepository.AddBook(book);
                return Ok();
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    Content = ex.Message,
                    StatusCode = 500
                };
            }
        }       
    }
}
