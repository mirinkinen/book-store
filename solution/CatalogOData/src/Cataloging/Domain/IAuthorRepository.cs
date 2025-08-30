namespace Cataloging.Domain;

public interface IAuthorRepository
{
    void AddAuthor(Author author);

    void Delete(Author author);

    ValueTask<Author?> GetAuthorById(Guid authorId, CancellationToken cancellationToken);

    Task<int> SaveChangesAsync();
}