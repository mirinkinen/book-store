using Common.Application.Authentication;

namespace Cataloging.Domain;

public interface IReadOnlyDbContext
{
    IQueryable<Author> GetAuthors(User user);
    IQueryable<Book> GetBooks(User user);
}