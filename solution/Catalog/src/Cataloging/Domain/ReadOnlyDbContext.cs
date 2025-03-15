using Common.Application.Authentication;

namespace Cataloging.Domain;

public class ReadOnlyDbContext : IReadOnlyDbContext
{
    private readonly IReadOnlyDbContextRepository _readOnlyDbContextRepository;

    public ReadOnlyDbContext(IReadOnlyDbContextRepository readOnlyDbContextRepository)
    {
        _readOnlyDbContextRepository = readOnlyDbContextRepository;
    }

    public IQueryable<Author> GetAuthors(User user) =>
        _readOnlyDbContextRepository.GetAuthorQuery()
            .Where(author => user.Organizations.Contains(author.OrganizationId));

    public IQueryable<Book> GetBooks(User user) =>
        _readOnlyDbContextRepository.GetBookQuery()
            .Where(book => user.Organizations.Contains(book.Author.OrganizationId));
}