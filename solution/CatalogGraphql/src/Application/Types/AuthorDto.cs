// using Application.AuthorQueries.GetAuthors;
// using HotChocolate;
// using HotChocolate.Types;
//
// namespace Application.Types;
//
// [ObjectType("Author")]
// public record AuthorDto
// {
//     public AuthorDto([GraphQLType<IdType>] Guid Id,
//         string FirstName,
//         string LastName,
//         DateOnly Birthdate,
//         Guid OrganizationId)
//     {
//         this.Id = Id;
//         this.FirstName = FirstName;
//         this.LastName = LastName;
//         this.Birthdate = Birthdate;
//         this.OrganizationId = OrganizationId;
//     }
//     
//     [Obsolete("Only for serialization", true)]
//     public AuthorDto() {}
//
//     /// <summary>
//     /// This is a resolver for a custom string field! It can return any string by any means necessary.
//     /// </summary>
//     public Task<string> AdditionalField(ScopedService scopedService)
//     {
//         return scopedService.GetHelloWorld();
//     }
//     
//     public Task<string> AnotherField(ScopedService scopedService)
//     {
//         return scopedService.GetHelloWorld();
//     }
//
//     public Guid Id { get; init; }
//     public string FirstName { get; init; }
//     public string LastName { get; init; }
//     public DateOnly Birthdate { get; init; }
//     public Guid OrganizationId { get; init; }
// }
//     
//     