namespace Domain
{
    /// <summary>
    /// Repository interface for Author entity
    /// </summary>
    public interface IAuthorRepository
    {
        Task<Author?> GetByIdAsync(Guid id);
        IQueryable<Author> GetQuery();
        Task<Author> AddAsync(Author author);
        Task<Author> UpdateAsync(Author author);
        Task<bool> DeleteAsync(Guid id);
    }
}
