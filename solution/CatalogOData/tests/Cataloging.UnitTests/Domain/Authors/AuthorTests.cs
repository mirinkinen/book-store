using AwesomeAssertions;
using Cataloging.Domain;
using FluentValidation;

namespace Cataloging.UnitTests.Domain.Authors;

[Trait("Category", "Author")]
public class AuthorTests
{
    [Fact]
    public void Author_WhenCreated_HasBasicInformation()
    {
        var firstName = "First name";
        var lastName = "Last name";
        var birthdate = DateTime.UtcNow;
        var organizationId = Guid.NewGuid();

        var author = new Author(firstName, lastName, birthdate, organizationId);

        author.FirstName.Should().Be(firstName);
        author.LastName.Should().Be(lastName);
        author.Birthdate.Should().Be(birthdate);
        author.OrganizationId.Should().Be(organizationId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Author_WithEmptyFirstName_ShouldThrowException(string? firstName)
    {
        var userId = Guid.NewGuid();
        var lastName = "Last name";
        var birthdate = DateTime.UtcNow;
        var organizationId = Guid.NewGuid();

        var constructor = () => new Author(firstName!, lastName, birthdate, organizationId);

        constructor.Should().Throw<ValidationException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Author_WithEmptyLastName_ShouldThrowException(string? lastName)
    {
        var userId = Guid.NewGuid();
        var firstName = "First name";
        var birthdate = DateTime.UtcNow;
        var organizationId = Guid.NewGuid();

        var constructor = () => new Author(firstName, lastName!, birthdate, organizationId);

        constructor.Should().Throw<ValidationException>();
    }

    [Fact]
    public void Author_WithEmptyOrganizationId_ShouldThrowException()
    {
        var userId = Guid.NewGuid();
        var firstName = "First name";
        var lastName = "Last name";
        var birthdate = DateTime.UtcNow;

        var constructor = () => new Author(firstName, lastName, birthdate, Guid.Empty);

        constructor.Should().Throw<ValidationException>();
    }
}