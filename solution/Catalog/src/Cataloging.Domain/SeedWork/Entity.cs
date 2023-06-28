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

        public Guid ModifiedBy { get; protected set; }

        protected Entity(Guid modifiedBy)
        {
            if (modifiedBy == default)
            {
                throw new ArgumentException("ModifiedBy must not be empty.");
            }

            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            ModifiedAt = CreatedAt;
            ModifiedBy = modifiedBy;
        }
    }
}