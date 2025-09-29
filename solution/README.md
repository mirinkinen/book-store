# Solution

## Local develoment

The start database, move to path `solution` folder. To start docker, run

```
docker compose up -d
```

To create databases, run

```
dotnet ef database update -s CatalogOData/src/Cataloging -c CatalogDbContext
dotnet ef database update -s CatalogGraphql/src/API -c CatalogDbContext
```

To seed development data, run

```
dotnet run --project CatalogOData/src/Cataloging -- seed-dev-data
dotnet run --project CatalogGraphql/src/API -- SEED-TEST-DATA
```

## Database migrations

To add new database migration, change the `<MigrationName>` and run

```
dotnet ef migrations add <MigrationName> -s CatalogOData/src/Cataloging -c CatalogDbContext -o Database/Migrations 
dotnet ef migrations add <MigrationName> -s CatalogGraphql/src/API -p CatalogGraphql/src/Infra -c CatalogDbContext -o Database/Migrations
```

## Cleaning up

To drop databases, run

```
dotnet ef database drop -s CatalogOData/src/Cataloging -c CatalogDbContext
dotnet ef database drop -s CatalogGraphql/src/API -c CatalogDbContext
```
