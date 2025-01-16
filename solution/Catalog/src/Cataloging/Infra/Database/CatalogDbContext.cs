using Cataloging.Domain;
using Cataloging.Infra.Database.EntityTypeConfigurations;
using Cataloging.Requests.Authors.Domain;
using Cataloging.Requests.Books.Domain;
using Common.Application.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Cataloging.Infra.Database;

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
        return SaveChangesAsync(acceptAllChangesOnSuccess).GetAwaiter().GetResult();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker
            .Entries<Entity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified);
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