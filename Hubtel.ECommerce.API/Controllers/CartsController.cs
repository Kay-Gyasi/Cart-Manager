using Hubtel.ECommerce.API.Core.Application.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hubtel.ECommerce.API.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly CartProcessor _processor;
        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        public CartsController(CartProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CartCommand command)
        {
            var result = await _processor.AddToCart(command);
            if (result == null) return NoContent();
            return CreatedAtAction(nameof(Get), new { userId = UserId }, result);
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> Remove(int itemId)
        {
            await _processor.RemoveFromCart(itemId);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? filter)
        {
            var result = await _processor.GetCart(filter);
            return Ok(result);
        }
    }
}
