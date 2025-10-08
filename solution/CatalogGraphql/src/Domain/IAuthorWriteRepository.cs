namespace Domain;

/// <summary>
/// Repository interface for Author entity
/// </summary>
public interface IAuthorWriteRepository : IWriteRepository<Author>
{
    Task<bool> AuthorWithNameExists(string firstName, string lastName, CancellationToken cancellationToken = default);
}