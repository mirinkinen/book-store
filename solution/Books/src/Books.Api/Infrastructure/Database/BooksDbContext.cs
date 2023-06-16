using Books.Api.Domain.Authors;
using Books.Api.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace Books.Api.Infrastructure.Database;

public class BooksDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }

    public BooksDbContext(DbContextOptions options) : base(options)
    {
    }
}