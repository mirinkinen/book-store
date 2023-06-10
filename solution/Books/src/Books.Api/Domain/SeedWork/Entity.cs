namespace Books.Api.Domain.SeedWork
{
    public abstract class Entity
    {
        public Guid Id { get; internal set; }

        public DateTimeOffset Created { get; internal set; }

        public DateTimeOffset Updated { get; internal set; }

        public DateTimeOffset Deleted { get; internal set; }
    }
}