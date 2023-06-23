using Audit.EntityFramework.Interceptors;
using Books.Domain.Authors;
using Books.Domain.Books;
using Books.Infrastructure.Database.EntityTypeConfigurations;
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

        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.AddInterceptors(new AuditCommandInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorEntityConfiguration).Assembly);
    }
}