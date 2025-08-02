using Common.API.Auditing;
using JasperFx;
using Ordering;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
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
app.UseAuthorization();
app.UseAuditLogging();
app.MapControllers();

// Opt into using JasperFx for command parsing
await app.RunJasperFxCommands(args);