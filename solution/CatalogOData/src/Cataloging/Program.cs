using Cataloging;
using Cataloging.API.Auditing;
using Common.API;
using JasperFx;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
ArgumentNullException.ThrowIfNull(connectionString);

builder.ConfigureServices(connectionString);

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

if (app.Environment.IsDevelopment())
{
    app.UseODataRouteDebug();
}

// Opt into using JasperFx for command parsing
await app.RunJasperFxCommands(args);