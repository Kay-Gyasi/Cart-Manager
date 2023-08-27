using Hubtel.ECommerce.API.Core.Application.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hubtel.ECommerce.API.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserProcessor _processor;

        public UsersController(UserProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _processor.LoginAsync(command);
            if (result == null) return BadRequest("Invalid command");
            return Ok(result);
        }
    }
}
