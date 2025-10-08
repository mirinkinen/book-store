using Infra.Database;
using TestData;

namespace API;

public partial class ArgumentExecutor
{
    public static async Task ExecuteArguments(WebApplication app, string[]? args)
    {
        // Do nothing, if no arguments.
        if (args is null || args.Length == 0)
        {
            return;
        }

        var command = args![0].ToUpperInvariant();
        switch (command)
        {
            case "SEED-TEST-DATA":
                using (var scope = app.Services.CreateScope())
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<ArgumentExecutor>>();

                    LogSeedingData(logger);
                    var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                    await DataSeeder.SeedDataAsync(dbContext);
                    LogDone(logger);
                    Environment.Exit(0);
                }

                break;
        }
    }

    [LoggerMessage(LogLevel.Information, "Seeding data...")]
    static partial void LogSeedingData(ILogger<ArgumentExecutor> logger);

    [LoggerMessage(LogLevel.Information, "Done.")]
    static partial void LogDone(ILogger<ArgumentExecutor> logger);
}