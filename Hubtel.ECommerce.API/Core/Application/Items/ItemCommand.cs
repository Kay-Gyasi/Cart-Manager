namespace Hubtel.ECommerce.API.Core.Application.Items
{
    public class ItemCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuantityAvailable { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
