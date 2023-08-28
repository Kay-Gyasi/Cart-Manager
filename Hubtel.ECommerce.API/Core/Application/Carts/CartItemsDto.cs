using Hubtel.ECommerce.API.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Hubtel.ECommerce.API.Core.Application.Carts
{
    public class CartItemsDto
    {
        public List<CartItemDto> Items { get; set; }

        public static explicit operator CartItemsDto(List<CartEntry> entries)
        {
            return new CartItemsDto
            {
                Items = entries.Select(x => new CartItemDto
                {
                    ItemId = x.ItemId,
                    Quantity = x.Quantity,
                    ItemName = x.ItemName,
                    UnitPrice = x.Item.UnitPrice
                }).ToList()
            };
        }
    }

    public class CartItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
