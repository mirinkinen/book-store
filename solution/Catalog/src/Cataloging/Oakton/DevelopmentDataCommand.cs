using Cataloging.Infra.Database;
using Cataloging.Infra.Database.Setup;
using Oakton;

namespace Cataloging.Oakton;

[Description("Initialize Catalog database with development time data", Name = "seed-dev-data")]
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