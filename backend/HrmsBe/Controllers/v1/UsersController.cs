
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrmsBe.Controllers.v1
{
   
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("/User")]
    //[ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProducts()
        {
            var products = new[] { "Product1", "Product2" }; // Example data
            return Ok(products);
        }
    }
}
