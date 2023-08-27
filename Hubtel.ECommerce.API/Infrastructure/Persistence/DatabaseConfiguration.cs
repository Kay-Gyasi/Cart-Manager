using Hubtel.ECommerce.API.Core.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hubtel.ECommerce.API.Infrastructure.Persistence
{
    public class DatabaseConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Audit, a =>
            {
                a.Property(x => x!.CreatedAt)
                    .IsRequired()
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                a.Property(x => x!.UpdatedAt)
                    .IsRequired();
                a.Property(x => x!.CreatedBy)
                    .IsRequired()
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                a.Property(x => x!.UpdatedBy)
                    .IsRequired();
                a.Property(x => x!.Status)
                    .HasConversion(new EnumToStringConverter<EntityStatus>());
            });
            builder.HasQueryFilter(x => x.Audit!.Status == EntityStatus.Normal);
        }
    }
}
