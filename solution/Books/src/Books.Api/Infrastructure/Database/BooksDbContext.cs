using Books.Api.Domain.Authors;
using Microsoft.EntityFrameworkCore;

namespace Books.Api.Infrastructure.Database;

public class BooksDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; }

    public BooksDbContext(DbContextOptions options) : base(options)
    {
    }
}