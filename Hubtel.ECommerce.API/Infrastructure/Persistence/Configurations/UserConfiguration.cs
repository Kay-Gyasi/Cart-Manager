using Hubtel.ECommerce.API.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hubtel.ECommerce.API.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public virtual void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(u => u.Cart)
                    .WithOne(c => c!.User)
                    .HasForeignKey<Cart>(c => c.UserId);
        }
    }
}
