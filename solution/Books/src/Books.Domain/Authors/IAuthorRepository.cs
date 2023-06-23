namespace Books.Domain.Authors;

public interface IAuthorRepository
{
    void AddAuthor(Author author);

    Task<int> SaveChangesAsync();
}