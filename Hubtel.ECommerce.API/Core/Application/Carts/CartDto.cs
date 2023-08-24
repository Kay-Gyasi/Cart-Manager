using Hubtel.ECommerce.API.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Hubtel.ECommerce.API.Core.Application.Carts
{
    public class CartDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartEntryDto> Items { get; set; }

        public static explicit operator CartDto(Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.CartEntries.Select(x => new CartEntryDto
                {
                    Id = x.Id,
                    ItemId = x.ItemId,
                    ItemName = x.Item.Name,
                    Quantity = x.Quantity
                }).ToList()
            };
        }
    }

    public class CartEntryDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
