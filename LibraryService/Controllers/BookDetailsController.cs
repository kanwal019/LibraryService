using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookDetailsController : ControllerBase
    {
        private readonly ILogger<BookDetailsController> _logger;

        public BookDetailsController(ILogger<BookDetailsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> FetchAllBooks()
        {
            try
            {
                return "All Books";
            }
            catch (Exception ex)
            {
                _ = ex;
                return ex.Message;
            }
        }
    }
}
