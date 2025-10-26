# Solution

## Local develoment

The start database, move to path `solution` folder. To start docker, run

```
docker compose up -d
```

To create databases, run

```
dotnet ef database update -s Catalog/src/API -c CatalogDbContext
dotnet ef database update -s Orders/src/API -c OrdersDbContext
```

To seed development data, run

```
dotnet run --project Catalog/src/API -- seed-test-data
dotnet run --project Orders/src/API -- seed-test-data
```

## Database migrations

To add new database migration, change the `<MigrationName>` and run

```
dotnet ef migrations add <MigrationName> -s Catalog/src/API -p Catalog/src/Infra -c CatalogDbContext -o Database/Migrations
dotnet ef migrations add <MigrationName> -s Orders/src/API -p Orders/src/Infra -c OrdersDbContext -o Database/Migrations
```

## Cleaning up

To drop databases, run

```
dotnet ef database drop -s Catalog/src/API -c CatalogDbContext
dotnet ef database drop -s Orders/src/API -c OrdersDbContext
```
