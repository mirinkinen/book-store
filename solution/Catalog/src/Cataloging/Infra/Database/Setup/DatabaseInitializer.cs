using Microsoft.EntityFrameworkCore;
using Oakton.Resources;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Cataloging.Infra.Database.Setup;

public class DatabaseInitializer : IStatefulResource
{
    private readonly CatalogDbContext _dbContext;

    public string Type => "CatalogDatabase";

    public string Name => "DatabaseInitializer";

    public DatabaseInitializer(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Check(CancellationToken token)
    {
        var canConnect = await _dbContext.Database.CanConnectAsync(token);

        if (!canConnect)
        {
            throw new InvalidOperationException("Unable to connect to Catalog database.");
        }
    }

    public Task ClearState(CancellationToken token)
    {
        _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE dbo.Books", token);
        _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE dbo.Authors", token);

        return Task.CompletedTask;
    }

    public Task Teardown(CancellationToken token)
    {
        return _dbContext.Database.EnsureDeletedAsync(token);
    }

    public async Task Setup(CancellationToken token)
    {
        await _dbContext.Database.MigrateAsync(token);
    }

    public async Task<IRenderable> DetermineStatus(CancellationToken token)
    {
        var table = new Table();
        table.AddColumns("Check");

        var canConnect = await _dbContext.Database.CanConnectAsync(token);
        if (!canConnect)
        {
            table.AddRow(new Markup($"[red]Connection failed.[/]"));
            return table;
        }

        table.AddRow(new Markup($"[green]Connection succeeded.[/]"));

        var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(token);
        var pendingCount = pendingMigrations.Count();

        table.AddRow(pendingCount > 0
            ? new Markup($"[red]Pending migrations: {pendingCount}[/]")
            : new Markup($"[green]No pending migrations.[/]"));

        return table;
    }
}