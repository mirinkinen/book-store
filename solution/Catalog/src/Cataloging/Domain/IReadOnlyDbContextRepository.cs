namespace Cataloging.Domain;

public interface IReadOnlyDbContextRepository
{
    IQueryable<Author> GetAuthorQuery();
    IQueryable<Book> GetBookQuery();
}