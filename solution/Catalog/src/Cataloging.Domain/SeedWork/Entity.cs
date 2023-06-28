namespace Cataloging.Domain.SeedWork
{
    /// <summary>
    /// Represents shared properties for all entities in this domain.
    /// </summary>
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime ModifiedAt { get; private set; }

        /// <summary>
        /// ModifiedBy is managed at infra layer to keep domain logic cleaner.
        /// </summary>
        public Guid ModifiedBy { get; internal set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            ModifiedAt = CreatedAt;
        }
    }
}