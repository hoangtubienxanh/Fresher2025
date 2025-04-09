# EF Core Assignment - Part 1 + 2

## Database Setup and Seeding

This project uses EF 9's `UseSeeding` method to seed the
database: [docs](https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding#configuration-options-useseeding-and-useasyncseeding-methods)

```sh
# Apply migrations and run seeding
dotnet ef database update
```

Seeding data for Departments table