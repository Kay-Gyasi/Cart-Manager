using Hubtel.ECommerce.API.Core.Application.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using static Hubtel.ECommerce.API.Core.Application.Carts.CartProcessor;

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
            return CreatedAtAction(nameof(Get), result);
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

        [AllowAnonymous]
        [HttpGet("items")]
        public async Task<IActionResult> GetAllCartItems(
            [FromQuery] string? phone, 
            [FromQuery] string? time,
            [FromQuery] string? quantity, 
            [FromQuery] string? itemName)
        {
            var filter = new Filter(phone, time, quantity, itemName);
            var result = await _processor.GetAllCartItems(filter);
            return Ok(result);
        }
    }
}
