using Books.Api.Domain.Books;
using Books.Api.Domain.SeedWork;
using FluentAssertions;

namespace Books.Api.Tests.Domain.SeedWork
{
    [Trait("Category", "SeedWork")]
    public class EntityTests
    {
        [Fact]
        public void Entity_WhenCreated_HasId()
        {
            var entity = (Entity)new Book("test", new DateTime(2020, 1, 1), Guid.NewGuid());

            entity.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void Entity_WhenCreated_HasUtcNowTimestamps()
        {
            var before = DateTimeOffset.UtcNow;
            var entity = (Entity)new Book("test", new DateTime(2020, 1, 1), Guid.NewGuid());
            var after = DateTimeOffset.UtcNow;

            entity.Created.Should().BeAfter(before).And.BeBefore(after);
            entity.Updated.Should().Be(entity.Created);
            entity.Deleted.Should().BeNull();
        }
    }
}