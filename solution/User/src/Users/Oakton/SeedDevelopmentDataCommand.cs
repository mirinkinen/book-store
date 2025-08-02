using JasperFx.CommandLine;
using Users.Infra.Database;
using Users.Infra.Database.Setup;

namespace Users.JasperFx;

[Description("Seed user database with development data", Name = "seed-dev-data")]
public class SeedDevelopmentDataCommand : JasperFxCommand<NetCoreInput>
{
    public override bool Execute(NetCoreInput input)
    {
        using var host = input.BuildHost();
        using var scope = host.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

        DataSeeder.SeedDataAsync(dbContext).GetAwaiter().GetResult();

        return true;
    }
}