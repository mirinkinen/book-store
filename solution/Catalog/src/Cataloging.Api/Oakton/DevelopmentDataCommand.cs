using Cataloging.Infrastructure.Database;
using Cataloging.Infrastructure.Database.Setup;
using Oakton;

namespace Cataloging.Api.Oakton;

[Description("Initialize Catalog database with development time data", Name = "initialize-dev-data")]
public class DevelopmentDataCommand : OaktonCommand<NetCoreInput>
{
    public override bool Execute(NetCoreInput input)
    {
        using var host = input.BuildHost();
        using var scope = host.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        DataSeeder.SeedDataAsync(dbContext).GetAwaiter().GetResult();

        return true;
    }
}