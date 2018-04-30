# How to use Entity Framework Core with Sqlite database in-memory mode in .Net Core

## Steps

1. Add Nuget package `Microsoft.EntityFrameworkCore.Sqlite` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/databae_demo/database_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.EntityFrameworkCore.Sqlite
    dotnet restore
    ```

2. Add `Microsoft.EntityFrameworkCore` namespace.

    > e.g. [EntityFrameworkSqliteInMemoryDemo.cs](../../demos/database_demo/EntityFrameworkSqliteInMemoryDemo.cs)
    ```csharp
    using Microsoft.EntityFrameworkCore;
    ```

3. [Defines data models and database context classes](https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite#create-the-model).

    > e.g. [EntityFrameworkSqliteInMemoryDemo.cs](../../demos/database_demo/EntityFrameworkSqliteInMemoryDemo.cs)

    * Data Models: `DemoEntity` class and `DemoNestedEntity` class.

    ```csharp
    public class DemoNestedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DemoEntity> SubEntities { get; set; }
    }

    public class DemoEntity
    {
        public int SubId { get; set; }
        public string SubName { get; set; }
        public DemoNestedEntity ParentEntity { get; set; }
    }
    ```

    * Database Context: `DemoContext` class.

        > The pre-configured database options for sqlite database should be provided in constructor, otherwise will [use in-memory database provider](how_to_use_ef_in_memory.md) by default.

        ```csharp
        public class DemoContext : DbContext
        {
            public DemoContext(DbContextOptions<DemoContext> options)
                : base(options)
            { }

            public DbSet<DemoNestedEntity> NestedEntities { get; set; }
            public DbSet<DemoEntity> Entities { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseInMemoryDatabase(databaseName: "in_memory_database");
                }
            }
        }
        ```

4. Create the sqlite in-memory database and the database context options.

    > e.g. [EntityFrameworkSqliteInMemoryDemo.cs](../../demos/database_demo/EntityFrameworkSqliteInMemoryDemo.cs)

    ```csharp
    SqliteConnection sqliteConnection = new SqliteConnection("DataSource=:memory:");
    sqliteConnection.Open();

    DbContextOptions<DemoContext> options =
        new DbContextOptionsBuilder<DemoContext>()
            .UseSqlite(sqliteConnection)
            .Options;
    ```

    > The sqlite in-memory database only exists while the connection opening, and will be destroyed after the connection closed.

5. [Insert and query data from database context](https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite#use-your-model).

    > e.g. [EntityFrameworkSqliteInMemoryDemo.cs](../../demos/database_demo/EntityFrameworkSqliteInMemoryDemo.cs)

    ```csharp
    DemoNestedEntity nestedEntity =
        new DemoNestedEntity { Name = "nested_entity_1" };
    List<DemoEntity> subEntities = new List<DemoEntity>
    {
        new DemoEntity { SubName = "sub_entity_1", ParentEntity = nestedEntity },
        new DemoEntity { SubName = "sub_entity_2", ParentEntity = nestedEntity },
        new DemoEntity { SubName = "sub_entity_3", ParentEntity = nestedEntity },
    };
    nestedEntity.SubEntities = subEntities;

    using (DemoContext db = new DemoContext(options))
    {
        db.NestedEntities.Add(nestedEntity);
        int count = db.SaveChanges();
        Console.WriteLine($"{count} records saved to database");

        Console.WriteLine("All nested entities in database:");
        foreach (DemoNestedEntity entity in db.NestedEntities)
        {
            Console.WriteLine($" - id: '{entity.Id}', name '{entity.Name}'");
        }

        Console.WriteLine("All sub entities in database:");
        foreach (DemoEntity entity in db.Entities)
        {
            Console.WriteLine($" - id: '{entity.SubId}', name '{entity.SubName}'");
        }
    }
    ```

## References

* [Entity Framework Core Quick Overview (docs.microsoft.com)](https://docs.microsoft.com/en-us/ef/core/)
* [Sqlite EF Core Database Provider (docs.microsoft.com)](https://docs.microsoft.com/en-us/ef/core/providers/sqlite/)
* [Testing with SQLite (docs.microsoft.com)](https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/sqlite)
* [Microsoft.EntityFrameworkCore Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore)
* [Microsoft.EntityFrameworkCore.Sqlite (nuget.org)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite)
* [EFCore.Sqlite.Core (github.com)](https://github.com/aspnet/EntityFrameworkCore/tree/dev/src/EFCore.Sqlite.Core)
* [Sqlite on .NET Core (www.bricelam.net)](http://www.bricelam.net/2015/04/29/sqlite-on-corefx.html)
* [Microsoft.Data.Sqlite (nuget.org)](https://www.nuget.org/packages/Microsoft.Data.Sqlite)
* [Microsoft.Data.Sqlite (github.com)](https://github.com/aspnet/Microsoft.Data.Sqlite)