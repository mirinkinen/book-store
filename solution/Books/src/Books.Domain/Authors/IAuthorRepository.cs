namespace Books.Domain.Authors;

public interface IAuthorRepository
{
    void AddAuthor(Author author);
    ValueTask<Author?> GetAuthorById(Guid authorId, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync();
}