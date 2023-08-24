using Hubtel.ECommerce.API.Core.Application.Carts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hubtel.ECommerce.API.Controllers
{
    public class CartsController : Controller
    {
        private readonly CartProcessor _processor;

        public CartsController(CartProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CartCommand command)
        {
            var result = await _processor.AddToCart(command);
            if (result == null) return NoContent();
            return CreatedAtAction(nameof(Get), new { userId = command.UserId }, result);
        }

        [HttpDelete("{userId}/{itemId}")]
        public async Task<IActionResult> Remove(int userId, int itemId)
        {
            await _processor.RemoveFromCart(itemId, userId);
            return NoContent();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId, [FromQuery] string? filter)
        {
            var result = await _processor.GetCart(userId, filter);
            return Ok(result);
        }
    }
}
