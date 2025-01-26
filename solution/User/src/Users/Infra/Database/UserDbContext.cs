using Common.Application.Authentication;
using Microsoft.EntityFrameworkCore;
using Users.Domain;
using User = Users.Domain.User;

namespace Users.Infra.Database;

public class UserDbContext : DbContext
{
    private readonly IUserService _userService;
    public DbSet<User> Users { get; set; }
    
    public DbSet<Address> Addresses { get; set; }

    public UserDbContext()
    {
    }
    
    public UserDbContext(DbContextOptions options, IUserService userService) : base(options)
    {
        _userService = userService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userEntity = modelBuilder.Entity<User>()
            .ToTable("User", "Users")
            .HasMany(u => u.Addresses)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.Id);

        var addressEntity = modelBuilder.Entity<Address>()
            .ToTable("User", "Addresses");
    }
}