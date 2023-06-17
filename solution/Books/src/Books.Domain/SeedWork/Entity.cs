namespace Books.Domain.SeedWork
{
    /// <summary>
    /// Represents shared properties for all entities in this domain.
    /// </summary>
    public abstract class Entity
    {
        public Guid Id { get; internal set; }

        public DateTime Created { get; internal set; }

        public DateTime Updated { get; internal set; }

        public DateTime? Deleted { get; internal set; }

        protected Entity()
        {
            var now = DateTime.UtcNow;

            Id = Guid.NewGuid();
            Created = now;
            Updated = now;
        }
    }
}