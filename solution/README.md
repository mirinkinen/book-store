# Solution

## Local develoment

The start database, move to path `solution` and run

```
docker compose up -d
```

in solution folder.

### Data seeding

Catalog component needs data seeding to work properly. To seed the data, move to path `solution/Catalog/src/Cataloging` and run

```
dotnet run -- seed-dev-data
```
