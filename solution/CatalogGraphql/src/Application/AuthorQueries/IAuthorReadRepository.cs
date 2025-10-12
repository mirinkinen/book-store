using Application.Services;
using Domain;

namespace Application.AuthorQueries;

public interface IAuthorReadRepository : IReadRepository<Author, AuthorNode>
{
}