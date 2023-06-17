using Books.Domain.Authors;
using Books.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Database;

public class BooksDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }

    public BooksDbContext(DbContextOptions options) : base(options)
    {
    }
}