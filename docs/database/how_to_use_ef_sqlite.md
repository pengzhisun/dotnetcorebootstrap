# How to use Entity Framework Core to access Sqlite database in .Net Core

## Steps

1. Add Nuget package `Microsoft.EntityFrameworkCore.Sqlite` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/databae_demo/database_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.EntityFrameworkCore.Sqlite
    dotnet restore
    ```

2. Add `Microsoft.EntityFrameworkCore` namespace.

    > e.g. [EntityFrameworkSqliteDemo.cs](../../demos/database_demo/EntityFrameworkSqliteDemo.cs)
    ```csharp
    using Microsoft.EntityFrameworkCore;
    ```

3. [Defines data models and database context classes](https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite#create-the-model).

    > e.g. [EntityFrameworkSqliteDemo.cs](../../demos/database_demo/EntityFrameworkSqliteDemo.cs)

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

        > `{DatabaseFileName}` sample: EntityFrameworkSqliteDemo.db

        ```csharp
        public class DemoContext : DbContext
        {
            public DbSet<DemoNestedEntity> NestedEntities { get; set; }
            public DbSet<DemoEntity> Entities { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite($"Data Source={DatabaseFileName}");
            }
        }
        ```

4. Optionally, [create Sqlite database file](https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite#create-the-database) for previous defined data models.

    * Add Nuget packages `Microsoft.EntityFrameworkCore.Design` reference via `dotnet add {project} package {package}` command.

        > `{project}` sample: demos/databae_demo/database_demo.csproj

        ```bash
        dotnet add {project} package Microsoft.EntityFrameworkCore.Design
        dotnet restore
        ```

    * Add dotnet cli tool reference `Microsoft.EntityFrameworkCore.Tools.DotNet` in csproj file.

        > e.g. [database_demo.csproj](demos/databae_demo/database_demo.csproj)

        ```xml
        <Project Sdk="Microsoft.NET.Sdk">
            ...
            <ItemGroup>
                <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.2" />
            </ItemGroup>
        </Project>
        ```

        > run `dotnet resotre` after csproj file modifed.

    * Run `dotnet ef` commands in the same directory of csproj file.

        > `{dataContext}` sample: DotNetCoreBootstrap.DatabaseDemo.EntityFrameworkSqliteDemo+DemoContext

        ```bash
        $"dotnet ef migrations add InitialCreate -c {dataContext}",
        $"dotnet ef database update -c {dataContext}",
        ```

        > The demo file [EntityFrameworkSqliteDemo.cs](../../demos/database_demo/EntityFrameworkSqliteDemo.cs) provides an example for how to generate Sqlite database file in runtime.

5. [Insert and query data from database context](https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite#use-your-model).

    > e.g. [EntityFrameworkSqliteDemo.cs](../../demos/database_demo/EntityFrameworkSqliteDemo.cs)

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
* [Sqlite EF Core Database Provider (docs.microsoft.com)](https://docs.microsoft.com/en-us/ef/core/providers/sqlite/)
* [Getting Started with EF Core on .NET Core Console App with a New database (docs.microsoft.com)](https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite)
* [Microsoft.EntityFrameworkCore Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore)
* [Microsoft.EntityFrameworkCore.Sqlite (nuget.org)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite)
* [Microsoft.EntityFrameworkCore.Design (nuget.org)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
* [Microsoft.EntityFrameworkCore.Tools.DotNet (nuget.org)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools.DotNet)
* [EFCore.Sqlite.Core (github.com)](https://github.com/aspnet/EntityFrameworkCore/tree/dev/src/EFCore.Sqlite.Core)
* [Sqlite on .NET Core (www.bricelam.net)](http://www.bricelam.net/2015/04/29/sqlite-on-corefx.html)
* [Microsoft.Data.Sqlite (nuget.org)](https://www.nuget.org/packages/Microsoft.Data.Sqlite)
* [Microsoft.Data.Sqlite (github.com)](https://github.com/aspnet/Microsoft.Data.Sqlite)
* [DB Browser for Sqlite (sqlitebrowser.org)](http://sqlitebrowser.org/)
* [Command Line Shell For Sqlite (www.sqlite.org)](http://www.sqlite.org/cli.html)