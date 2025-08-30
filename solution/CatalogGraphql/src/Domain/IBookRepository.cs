using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Application.Repositories
{
    /// <summary>
    /// Repository interface for Book entity
    /// </summary>
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<IEnumerable<Book>> GetByAuthorIdAsync(Guid authorId);
        Task<Book> AddAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task<bool> DeleteAsync(Guid id);
    }
}
