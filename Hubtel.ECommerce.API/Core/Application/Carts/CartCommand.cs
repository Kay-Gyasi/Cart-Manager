using System.Collections.Generic;

namespace Hubtel.ECommerce.API.Core.Application.Carts
{
    public class CartCommand
    {
        public List<CartEntryCommand> CartEntries { get; set; }
    }

    public class CartEntryCommand
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
