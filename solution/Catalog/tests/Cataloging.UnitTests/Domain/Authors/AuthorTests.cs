using Cataloging.Requests.Authors.Domain;
using Common.Domain;
using FluentAssertions;

namespace Cataloging.UnitTests.Domain.Authors;

[Trait("Category", "Author")]
public class AuthorTests
{
    [Fact]
    public void Author_WhenCreated_HasBasicInformation()
    {
        var firstName = "First name";
        var lastName = "Last name";
        var birthday = DateTime.UtcNow;
        var organizationId = Guid.NewGuid();

        var author = new Author(firstName, lastName, birthday, organizationId);

        author.FirstName.Should().Be(firstName);
        author.LastName.Should().Be(lastName);
        author.Birthday.Should().Be(birthday);
        author.OrganizationId.Should().Be(organizationId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Author_WithEmptyFirstName_ShouldThrowException(string firstName)
    {
        var userId = Guid.NewGuid();
        var lastName = "Last name";
        var birthday = DateTime.UtcNow;
        var organizationId = Guid.NewGuid();

        var constructor = () => new Author(firstName, lastName, birthday, organizationId);

        constructor.Should().Throw<DomainRuleException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Author_WithEmptyLastName_ShouldThrowException(string lastName)
    {
        var userId = Guid.NewGuid();
        var firstName = "First name";
        var birthday = DateTime.UtcNow;
        var organizationId = Guid.NewGuid();

        var constructor = () => new Author(firstName, lastName, birthday, organizationId);

        constructor.Should().Throw<DomainRuleException>();
    }

    [Fact]
    public void Author_WithEmptyOrganizationId_ShouldThrowException()
    {
        var userId = Guid.NewGuid();
        var firstName = "First name";
        var lastName = "Last name";
        var birthday = DateTime.UtcNow;

        var constructor = () => new Author(firstName, lastName, birthday, Guid.Empty);

        constructor.Should().Throw<DomainRuleException>();
    }
}