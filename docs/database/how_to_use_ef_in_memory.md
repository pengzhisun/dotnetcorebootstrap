# How to use Entity Framework Core with in-memory database provider in .Net Core

## Steps

1. Add Nuget package `Microsoft.EntityFrameworkCore.InMemory` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/databae_demo/database_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.EntityFrameworkCore.InMemory
    dotnet restore
    ```

2. Add `Microsoft.EntityFrameworkCore` namespace.

    > e.g. [EntityFrameworkInMemoryDemo.cs](../../demos/database_demo/EntityFrameworkInMemoryDemo.cs)
    ```csharp
    using Microsoft.EntityFrameworkCore;
    ```

3. [Defines data models and database context classes](https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite#create-the-model).

    > e.g. [EntityFrameworkInMemoryDemo.cs](../../demos/database_demo/EntityFrameworkInMemoryDemo.cs)

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

        > `{database_name}` example: in_memory_database

        ```csharp
        public class DemoContext : DbContext
        {
            public DbSet<DemoNestedEntity> NestedEntities { get; set; }
            public DbSet<DemoEntity> Entities { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: "{database_name}");
            }
        }
        ```

4. [Insert and query data from database context](https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite#use-your-model).

    > e.g. [EntityFrameworkInMemoryDemo.cs](../../demos/database_demo/EntityFrameworkInMemoryDemo.cs)

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

    using (DemoContext db = new DemoContext())
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
* [EF Core In-Memory Database Provider (docs.microsoft.com)](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/)
* [Testing with InMemory (docs.microsoft.com)](https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/in-memory)
* [Microsoft.EntityFrameworkCore Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore)
* [Microsoft.EntityFrameworkCore.InMemory (nuget.org)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory)
* [EFCore.InMemory (github.com)](https://github.com/aspnet/EntityFrameworkCore/tree/dev/src/EFCore.InMemory)