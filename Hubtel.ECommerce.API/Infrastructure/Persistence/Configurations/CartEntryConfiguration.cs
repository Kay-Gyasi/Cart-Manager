using Hubtel.ECommerce.API.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hubtel.ECommerce.API.Infrastructure.Persistence.Configurations
{
    public class CartEntryConfiguration : DatabaseConfiguration<CartEntry, int>
    {
        public override void Configure(EntityTypeBuilder<CartEntry> builder)
        {
            base.Configure(builder);
            builder.Ignore(x => x.ItemName);
        }
    }
}
