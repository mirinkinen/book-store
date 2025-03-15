namespace Cataloging.Domain;

public interface IQueryAuthorizerRepository
{
    IQueryable<Author> GetAuthorQuery();
    IQueryable<Book> GetBookQuery();
}