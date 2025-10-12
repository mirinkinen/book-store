using Application.Services;
using Domain.Authors;

namespace Application.AuthorQueries;

public interface IAuthorReadRepository : IReadRepository<Author, AuthorNode>
{
}