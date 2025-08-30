using AwesomeAssertions;
using Cataloging.Domain;
using Common.Domain;

namespace Cataloging.UnitTests.Domain.SeedWork;

[Trait("Category", "SeedWork")]
public class EntityTests
{
    [Fact]
    public void Entity_WhenCreated_HasId()
    {
        var entity = (Entity)new Book(Guid.NewGuid(), "test", new DateTime(2020, 1, 1), 10);

        entity.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Entity_WhenCreated_HasBasicInformation()
    {
        var before = DateTime.UtcNow;
        var entity = (Entity)new Book(Guid.NewGuid(), "test", new DateTime(2020, 1, 1), 10);
        var after = DateTime.UtcNow;

        entity.CreatedAt.Should().BeAfter(before).And.BeBefore(after);
        entity.ModifiedAt.Should().Be(entity.CreatedAt);
    }
}