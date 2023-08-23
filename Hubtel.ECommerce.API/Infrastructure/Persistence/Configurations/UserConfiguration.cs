using Hubtel.ECommerce.API.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hubtel.ECommerce.API.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : DatabaseConfiguration<User, int>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.HasOne(u => u.Cart)
                    .WithOne(c => c.User)
                    .HasForeignKey<Cart>(c => c.UserId);
        }
    }
}
