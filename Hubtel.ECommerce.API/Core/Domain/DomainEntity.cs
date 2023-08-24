using System;

namespace Hubtel.ECommerce.API.Core.Domain
{
    public abstract class Entity
    {
        public int Id { get; protected set; }
        public Audit? Audit { get; set; }
        public Entity AuditAs(Audit audit)
        {
            Audit = audit;
            return this;
        }
    }

    public class Audit : ValueObject
    {
        private Audit()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            CreatedBy = "admin";
            UpdatedBy = "admin";
        }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public string UpdatedBy { get; private set; }
        public EntityStatus Status { get; private set; } = EntityStatus.Normal;

        public static Audit Create() => new Audit();

        public Audit WasCreatedBy(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return this;
            CreatedBy = name;
            return this;
        }

        public Audit Update(string name)
        {
            UpdatedBy = name;
            UpdatedAt = DateTime.UtcNow;
            return this;
        }

        public Audit Delete()
        {
            Status = EntityStatus.Deleted;
            return this;
        }

        public Audit Archive()
        {
            Status = EntityStatus.Archived;
            return this;
        }
    }

    public enum EntityStatus
    {
        Normal = 1,
        Archived,
        Deleted
    }
}
