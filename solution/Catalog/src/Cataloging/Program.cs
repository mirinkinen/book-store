using Cataloging;
using Common.API;
using Common.API.Auditing;
using Microsoft.AspNetCore.OData;
using Oakton;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("CatalogConnectionString");
ArgumentNullException.ThrowIfNull(connectionString);

ServiceConfigurator.ConfigureServices(builder, connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ProblemDetailsMiddleware>();
app.UseAuthorization();
app.UseAuditLogging();
app.MapControllers();
app.MapGraphQL();

if (app.Environment.IsDevelopment())
{
    app.UseODataRouteDebug();
}

// Opt into using Oakton for command parsing
await app.RunOaktonCommands(args);