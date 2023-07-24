using Cataloging.Api;
using Common.Api.Auditing;
using Oakton;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("CatalogConnectionString");
ArgumentNullException.ThrowIfNull(connectionString);

ServiceRegistrar.RegisterServices(builder, connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuditLogging();
app.MapControllers();

// Opt into using Oakton for command parsing
await app.RunOaktonCommands(args);