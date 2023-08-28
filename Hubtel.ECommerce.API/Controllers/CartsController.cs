using Hubtel.ECommerce.API.Core.Application.Carts;
using Hubtel.ECommerce.API.Core.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
            if (result.IsT1) return ErrorResponse(result.AsT1);
            return CreatedAtAction(nameof(Get), result.AsT0);
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> Remove(int itemId)
        {
            var result = await _processor.RemoveFromCart(itemId);
            if (result.IsT1) return ErrorResponse(result.AsT1);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? filter)
        {
            var result = await _processor.GetCart(filter);
            if (result.IsT1) return ErrorResponse(result.AsT1);
            return Ok(result.AsT0);
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
            if (result.IsT1) return ErrorResponse(result.AsT1);
            return Ok(result.AsT0);
        }

        private JsonResult ErrorResponse(Exception exception)
        {
            return new JsonResult(new { exception.Message })
            {
                StatusCode = exception is InvalidQuantityException ?
                                                    StatusCodes.Status400BadRequest
                                                    : StatusCodes.Status500InternalServerError
            };
        }
    }
}
