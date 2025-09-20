namespace Domain
{
    /// <summary>
    /// Repository interface for Author entity
    /// </summary>
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<bool> AuthorWithNameExists(string firstName, string lastName, CancellationToken cancellationToken = default);
    }
}
