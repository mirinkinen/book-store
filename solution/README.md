# Solution

## Local develoment


The start database, move to path `solution` folder. To start docker, run

```
docker compose up -d
```

To create databases, run

```
dotnet ef database update -s Catalog\src\Cataloging
dotnet ef database update -s Order\src\Ordering
dotnet ef database update -s User\src\Users
```

To seed development data, run 

```
dotnet run --project Catalog/src/Cataloging -- seed-dev-data
dotnet run --project User/src/Users -- seed-dev-data
```

## Database migrations

To add new database migration, change the `<MigrationName>` and run

```
dotnet ef migrations add -c CatalogDbContext -s Catalog\src\Cataloging -o Database\Migrations\ <MigrationName>
dotnet ef migrations add -c OrderDbContext -s Order\src\Ordering -o Database\Migrations\ <MigrationName>
```

## Cleaning up

To drop databases, run

```
dotnet ef database drop -s Catalog\src\Cataloging
dotnet ef database drop -s Order\src\Ordering
```
