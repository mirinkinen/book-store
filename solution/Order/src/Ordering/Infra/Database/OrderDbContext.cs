using Common.Application.Authentication;
using Common.Domain;
using Microsoft.EntityFrameworkCore;
using Ordering.Requests.Orders.Domain.Orders;
using Ordering.Requests.Orders.Infra;

namespace Ordering.Infra.Database;

public class OrderDbContext : DbContext
{
    private readonly IUserService _userService;

    public DbSet<Order> Orders { get; set; }

    public OrderDbContext(DbContextOptions options, IUserService userService) : base(options)
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
            .Entries<ITimestamped>()
            .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified);
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderEntityConfiguration).Assembly);
    }
}