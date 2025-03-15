using Common.Application.Authentication;
using Microsoft.EntityFrameworkCore;
using Users.Domain;
using User = Users.Domain.User;

namespace Users.Infra.Database;

public class UserDbContext : DbContext
{
    private readonly IUserAccessor _userAccessor;
    public DbSet<User> Users { get; set; }
    
    public DbSet<Address> Addresses { get; set; }

    public UserDbContext()
    {
    }
    
    public UserDbContext(DbContextOptions options, IUserAccessor userAccessor) : base(options)
    {
        _userAccessor = userAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userEntity = modelBuilder.Entity<User>()
            .ToTable("Users", "User");

        var addressEntity = modelBuilder.Entity<Address>()
            .ToTable("Addresses", "User")
            .HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId);

        var subscriptionEntity = modelBuilder.Entity<Subscription>()
            .ToTable("Subscriptions", "User")
            .HasOne(s => s.User)
            .WithOne(u => u.Subscription);
    }
}