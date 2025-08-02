using JasperFx.CommandLine;
using Users.Infra.Database;
using Users.Infra.Database.Setup;

namespace Users.JasperFx;

[Description("Truncates user database", Name = "truncate-dev-data")]
public class TruncateDevelopmentDataCommand : JasperFxCommand<NetCoreInput>
{
    public override bool Execute(NetCoreInput input)
    {
        using var host = input.BuildHost();
        using var scope = host.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

        DataRemover.RemoveDataAsync(dbContext).GetAwaiter().GetResult();

        return true;
    }
}