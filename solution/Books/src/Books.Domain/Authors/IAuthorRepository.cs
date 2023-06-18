using Books.Domain.Authors;

namespace Books.Application.Requests.Authors.AddAuthor;

public interface IAuthorRepository
{
    void AddAuthor(Author author);

    Task<int> SaveChangesAsync();
}