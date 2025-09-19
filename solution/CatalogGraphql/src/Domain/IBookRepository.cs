namespace Domain
{
    /// <summary>
    /// Repository interface for Book entity
    /// </summary>
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id);
        Task<IQueryable<Book>> GetQueryAsync();
        Task<IEnumerable<Book>> GetByAuthorIdAsync(Guid authorId);
        Task<Book> AddAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task<bool> DeleteAsync(Guid id);
    }
}
