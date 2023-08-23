using Hubtel.ECommerce.API.Core.Application.Carts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hubtel.ECommerce.API.Presentation.Controllers
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
            return Ok();
        }
        
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> Remove(int itemId)
        {
            return NoContent() ;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }
    }
}
