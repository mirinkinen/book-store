using Books.Domain.Books;
using Books.Domain.SeedWork;
using FluentAssertions;

namespace Books.Domain.UnitTests.SeedWork;

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
        var entity = (Entity)new Book("test", new DateTime(2020, 1, 1), Guid.NewGuid(), Guid.NewGuid());

        entity.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Entity_WhenCreated_HasBasicInformation()
    {
        var modifiedBy = Guid.NewGuid();
        var before = DateTime.UtcNow;
        var entity = (Entity)new Book("test", new DateTime(2020, 1, 1), Guid.NewGuid(), modifiedBy);
        var after = DateTime.UtcNow;

        entity.CreatedAt.Should().BeAfter(before).And.BeBefore(after);
        entity.ModifiedAt.Should().Be(entity.CreatedAt);
        entity.ModifiedBy.Should().Be(modifiedBy);
    }
}