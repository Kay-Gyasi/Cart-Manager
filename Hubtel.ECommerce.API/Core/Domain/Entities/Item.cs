using System.Collections.Generic;

namespace Hubtel.ECommerce.API.Core.Domain.Entities
{
    public class Item : Entity
    {
        private Item(string name, int quantityAvailable, decimal unitPrice)
        {
            Name = name;
            QuantityAvailable = quantityAvailable;
            UnitPrice = unitPrice;
        }

        public string Name { get; private set; }
        public int QuantityAvailable { get; private set; }
        public decimal UnitPrice { get; private set; }
        public bool IsInStock => QuantityAvailable > 0;

        private readonly List<Cart> _carts = new List<Cart>();
        public IReadOnlyList<Cart> Carts => _carts.AsReadOnly();

        public static Item Create(string name, int quantityAvailable, decimal unitPrice)
            => new Item(name, quantityAvailable, unitPrice);

        public Item WithName(string name)
        {
            Name = name;
            return this;
        }

        public Item HasQuantityAvailable(int quantityAvailable)
        {
            QuantityAvailable = quantityAvailable;
            return this;
        }

        public Item HasUnitPrice(decimal unitPrice)
        {
            UnitPrice = unitPrice;
            return this;
        }
    }
}
