using Application.Common;
using Domain;

namespace Application.AuthorQueries;

public interface IAuthorReadRepository : IReadRepository<Author, AuthorNode>
{
}