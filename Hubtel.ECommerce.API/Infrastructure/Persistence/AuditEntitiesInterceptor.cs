using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Hubtel.ECommerce.API.Core.Domain;

namespace Hubtel.ECommerce.API.Infrastructure.Persistence
{
    public sealed class AuditEntitiesInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuditEntitiesInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var dbContext = eventData.Context;
            if (dbContext is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            string username = "sysadmin";

            foreach (EntityEntry entry in dbContext.ChangeTracker.Entries()
                         .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
            {
                if (!(entry.Entity is Entity entity)) continue;
                entity.Audit ??= Audit.Create();
                entity.Audit.WasCreatedBy(username);
                entity.Audit.Update(username);
            }

            foreach (var entry in dbContext.ChangeTracker.Entries()
                         .Where(x => x.State is EntityState.Deleted))
            {
                if (!(entry.Entity is Entity entity)) continue;
                entity.Audit?.Update(username);
                entity.Audit?.Delete();
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
