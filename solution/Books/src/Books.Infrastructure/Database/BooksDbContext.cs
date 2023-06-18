using Audit.EntityFramework.Interceptors;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        optionsBuilder.AddInterceptors(new AuditCommandInterceptor());
    }
}