using Microsoft.EntityFrameworkCore;
using Oakton;
using Users;
using Users.Infra.Database;
using Users.Infra.Database.Setup;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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
await app.RunOaktonCommands(args);