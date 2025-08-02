using Cataloging.Infra.Database;
using Cataloging.Infra.Database.Setup;
using JasperFx.CommandLine;

namespace Cataloging.JasperFx;

[Description("Seed catalog database with development data", Name = "seed-dev-data")]
public class SeedDevelopmentDataCommand : JasperFxCommand<NetCoreInput>
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