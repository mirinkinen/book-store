using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using Cataloging.Domain.Books;
using Cataloging.Domain.SeedWork;
using Cataloging.Infrastructure.Database.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Cataloging.Infrastructure.Database;

public class CatalogDbContext : DbContext
{
    private readonly IUserService _userService;

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }

    public CatalogDbContext(DbContextOptions options, IUserService userService) : base(options)
    {
        _userService = userService;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new NotImplementedException("Use SaveChangesAsync");
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries<Entity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        var user = _userService.GetUser();

        foreach (var entityEntry in entities)
        {
            entityEntry.Entity.ModifiedBy = user.Id;
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorEntityConfiguration).Assembly);
    }
}