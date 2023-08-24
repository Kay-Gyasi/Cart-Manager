using Hubtel.ECommerce.API.Core.Domain.Entities;

namespace Hubtel.ECommerce.API.Core.Application.Items
{
    public class ItemDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int QuantityAvailable { get; set; }
        public decimal UnitPrice { get; set; }

        public static explicit operator ItemDto(Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                QuantityAvailable = item.QuantityAvailable,
                UnitPrice = item.UnitPrice,
            };
        }
    }
}
