using Application.Services;
using Domain;
using Domain.Authors;

namespace Application.AuthorQueries;

public interface IAuthorReadRepository : IReadRepository<Author, AuthorNode>
{
}