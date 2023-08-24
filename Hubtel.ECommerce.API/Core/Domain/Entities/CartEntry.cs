namespace Hubtel.ECommerce.API.Core.Domain.Entities
{
    public class CartEntry : Entity
    {
        private CartEntry(int itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }

        public int ItemId { get; private set; }
        public int Quantity { get; private set; }
        public string ItemName => Item.Name;
        public Item Item { get; private set; }

        public static CartEntry Create(int itemId, int quantity)
            => new CartEntry(itemId, quantity);

        public CartEntry HasQuantity(int quantity)
        {
            Quantity += quantity;
            return this;
        }
    }
}
