using Microsoft.EntityFrameworkCore;

namespace Users.Infra.Database.Setup;

public class DatabaseInitializer
{
    private readonly UserDbContext _dbContext;

    public string Type => "UserDatabase";

    public string Name => "DatabaseInitializer";

    public DatabaseInitializer(UserDbContext dbContext)
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
        _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE dbo.Users", token);
        _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE dbo.Addresses", token);

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
}