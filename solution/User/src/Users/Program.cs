using Microsoft.EntityFrameworkCore;
using Users;
using Users.Infra.Database;
using Users.Infra.Database.Setup;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (args.Length != 0 && args[0] == "seed-dev-data")
{
    var optionsBuilder = new DbContextOptionsBuilder();
    optionsBuilder.UseSqlServer(connectionString);
    await using var dbContext = new UserDbContext(optionsBuilder.Options, new SystemUserService());
    await dbContext.Database.MigrateAsync(CancellationToken.None);
    await DataSeeder.SeedDataAsync(dbContext);
    
    return;
}

ArgumentNullException.ThrowIfNull(connectionString);

builder.ConfigureServices(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapGraphQL();

// Opt into using Oakton for command parsing
await app.RunAsync();