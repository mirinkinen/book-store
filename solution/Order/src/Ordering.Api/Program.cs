using Ordering.Api;
using Common.Api.Auditing;
using Oakton;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OrderConnectionString");
ArgumentNullException.ThrowIfNull(connectionString);

ServiceRegistrar.RegisterApiServices(builder, connectionString);
Ordering.Application.ServiceRegistrar.RegisterApplicationServices(builder.Services);
Ordering.Infrastructure.ServiceRegistrar.RegisterInfrastructureServices(builder.Services, connectionString);

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
