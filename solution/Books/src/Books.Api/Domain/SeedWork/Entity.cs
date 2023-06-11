namespace Books.Api.Domain.SeedWork
{
    /// <summary>
    /// Represents shared properties for all entities in this domain.
    /// </summary>
    public abstract class Entity
    {
        public Guid Id { get; internal set; }

        public DateTimeOffset Created { get; internal set; }

        public DateTimeOffset Updated { get; internal set; }

        public DateTimeOffset? Deleted { get; internal set; }

        protected Entity()
        {
            var now = DateTimeOffset.UtcNow;
            
            Id = Guid.NewGuid();
            Created = now;
            Updated = now;
        }
    }
}