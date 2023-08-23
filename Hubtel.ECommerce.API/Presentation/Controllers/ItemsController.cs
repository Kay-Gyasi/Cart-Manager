﻿using Hubtel.ECommerce.API.Core.Application.Items;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hubtel.ECommerce.API.Presentation.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ItemProcessor _processor;

        public ItemsController(ItemProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> Get(int itemId)
        {
            return Ok();
        }
    }
}
