using Application.AuthorQueries.GetAuthors;
using HotChocolate;
using HotChocolate.Types;

namespace Application.Types;

[ObjectType("Author")]
public record AuthorDto(
    [GraphQLType<IdType>] Guid Id,
    string FirstName,
    string LastName,
    DateTime Birthdate,
    Guid OrganizationId)
{
    /// <summary>
    /// This is a resolver for a custom string field! It can return any string by any means necessary.
    /// </summary>
    public Task<string> AdditionalField(ScopedService scopedService)
    {
        return scopedService.GetHelloWorld();
    }
    
    public Task<string> AnotherField(ScopedService scopedService)
    {
        return scopedService.GetHelloWorld();
    }
}
    
    