using System.Collections.Generic;
using System.Linq;

namespace Hubtel.ECommerce.API.Core.Domain.Entities
{
    public class Cart : AggregateRoot
    {
        private Cart() { }
        private Cart(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
        public User User { get; private set; }

        private readonly List<CartEntry> _cartEntries = new List<CartEntry>();
        public IReadOnlyList<CartEntry> CartEntries => _cartEntries.AsReadOnly();

        public static Cart Create(int userId) => new Cart(userId);

        public Cart AddToCart(IEnumerable<CartEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (_cartEntries.Any(x => x.ItemId == entry.ItemId))
                {
                    var originalEntry = _cartEntries.First(x => x.ItemId == entry.ItemId);
                    originalEntry.HasQuantity(entry.Quantity); // factor in available quantity in stock
                    continue;
                }

                _cartEntries.Add(entry);
            }
            return this;
        }
        
        public Cart RemoveFromCart(int itemId)
        {
            var entry = _cartEntries.SingleOrDefault(x => x.ItemId == itemId);
            if (entry == null) return this;

            _cartEntries.Remove(entry);
            return this;
        }
    }
}
