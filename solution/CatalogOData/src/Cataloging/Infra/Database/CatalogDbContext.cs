using Cataloging.Domain;
using Common.Application.Authentication;
using Common.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Cataloging.Infra.Database;

public class CatalogDbContext : DbContext
{
    private readonly IUserAccessor _userAccessor;

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }

    public CatalogDbContext(DbContextOptions options, IUserAccessor userAccessor) : base(options)
    {
        _userAccessor = userAccessor;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return SaveChangesAsync(acceptAllChangesOnSuccess).GetAwaiter().GetResult();
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;

        var entities = ChangeTracker
            .Entries<Entity>()
            .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified);

        var user = await _userAccessor.GetUser();

        foreach (var entityEntry in entities)
        {
            entityEntry.Entity.ModifiedBy = user.Id;
            entityEntry.Entity.ModifiedAt = now;
        }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}