using Audit.EntityFramework;
using Books.Domain.Authors;
using Books.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Database;

public class AuditBooksDbContext : AuditDbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }

    public AuditBooksDbContext(DbContextOptions<AuditBooksDbContext> options) : base(options)
    {
    }
}