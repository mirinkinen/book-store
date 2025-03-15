using Common.Application.Authentication;
using Common.Domain;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain;
using System.Reflection;

namespace Ordering.Infra.Database;

public class OrderDbContext : DbContext
{
    private readonly IUserAccessor _userAccessor;

    public DbSet<Order> Orders { get; set; }

    public OrderDbContext(DbContextOptions options, IUserAccessor userAccessor) : base(options)
    {
        _userAccessor = userAccessor;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return SaveChangesAsync(acceptAllChangesOnSuccess).GetAwaiter().GetResult();
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker
            .Entries<ITimestamped>()
            .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified);
        
        var user = await _userAccessor.GetUser();

        foreach (var entityEntry in entities)
        {
            entityEntry.Entity.ModifiedBy = user.Id;
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