# Solution

## Local develoment


The start database, move to path `solution` folder. To start docker, run

```
docker compose up -d
```

To create databases, run

```
dotnet ef database update -s Catalog/src/Cataloging -c CatalogDbContext
dotnet ef database update -s Order/src/Ordering -c OrderDbContext
dotnet ef database update -s User/src/Users -c UserDbContext 
```

To seed development data, run 

```
dotnet run --project Catalog/src/Cataloging -- seed-dev-data
dotnet run --project User/src/Users -- seed-dev-data
```

## Database migrations

To add new database migration, change the `<MigrationName>` and run

```
dotnet ef migrations add -s Catalog/src/Cataloging -c CatalogDbContext  -o Database/Migrations/ <MigrationName>
dotnet ef migrations add -s Order/src/Ordering -c OrderDbContext -o Database/Migrations/ <MigrationName>
dotnet ef migrations add -s User/src/Users -c UserDbContext -o Database/Migrations/ <MigrationName>
```

## Cleaning up

To drop databases, run

```
dotnet ef database drop -s Catalog/src/Cataloging -c CatalogDbContext
dotnet ef database drop -s Order/src/Ordering -c OrderDbContext
dotnet ef database drop -s User/src/Users -c UserDbContext
```
