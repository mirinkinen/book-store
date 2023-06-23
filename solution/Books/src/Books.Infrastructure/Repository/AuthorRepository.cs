using Books.Domain.Authors;
using Books.Infrastructure.Database;

namespace Books.Infrastructure.Repository;

internal class AuthorRepository : IAuthorRepository
{
    private readonly BooksDbContext _booksDbContext;

    public AuthorRepository(BooksDbContext booksDbContext)
    {
        _booksDbContext = booksDbContext;
    }

    public void AddAuthor(Author author)
    {
        _booksDbContext.Add(author);
    }

    public ValueTask<Author?> GetAuthorById(Guid authorId, CancellationToken cancellationToken)
    {
        return _booksDbContext.FindAsync<Author>(authorId, cancellationToken);
    }

    public Task<int> SaveChangesAsync()
    {
        return _booksDbContext.SaveChangesAsync();
    }
}