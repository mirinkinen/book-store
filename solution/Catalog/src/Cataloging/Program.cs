using Cataloging.API;
using Cataloging.Schema;
using Common.API.Auditing;
using Microsoft.AspNetCore.OData;
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
app.UseMiddleware<ProblemDetailsMiddleware>();
app.UseAuthorization();
app.UseAuditLogging();
app.MapControllers();
app.UseGraphQL<CatalogSchema>();
app.UseGraphQLPlayground();
app.UseGraphQLGraphiQL();
app.UseGraphQLAltair();

if (app.Environment.IsDevelopment())
{
    app.UseODataRouteDebug();
}

// Opt into using Oakton for command parsing
await app.RunOaktonCommands(args);