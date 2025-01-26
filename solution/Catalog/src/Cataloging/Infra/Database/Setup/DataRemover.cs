using Cataloging.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Users.Infra.Database.Setup;

public static class DataRemover
{
    public static async Task RemoveDataAsync(CatalogDbContext catalogDbContext)
    {
        ArgumentNullException.ThrowIfNull(catalogDbContext);

        await catalogDbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Catalog.Book");
        await catalogDbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Catalog.Author");
    }
}