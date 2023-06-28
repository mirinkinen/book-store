using Cataloging.Domain.Books;
using Cataloging.Domain.SeedWork;
using FluentAssertions;

namespace Cataloging.Domain.UnitTests.SeedWork;

[Trait("Category", "SeedWork")]
public class EntityTests
{
    [Fact]
    public void Entities_DoesNotHavePublicSetters()
    {
        typeof(Entity).Assembly.GetTypes()
            .Where(type => typeof(Entity).IsAssignableFrom(type))
            .SelectMany(type => type.GetProperties())
            .Select(propertyInfo => propertyInfo.GetSetMethod())
            .Where(methodInfo => methodInfo != null && methodInfo.IsPublic)
            .Should().BeEmpty("domain entities should not have public setters");
    }

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