/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   EntityFrameworkSqliteInMemoryDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core Entity Framework for Sqlite in-memory mode demos.
 * Reference:   https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/sqlite
 *              https://docs.microsoft.com/en-us/ef/core/providers/sqlite/
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore
 *              https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite
 *              https://github.com/aspnet/EntityFrameworkCore/tree/dev/src/EFCore.Sqlite.Core
 *              http://www.bricelam.net/2015/04/29/sqlite-on-corefx.html
 *              https://www.nuget.org/packages/Microsoft.Data.Sqlite
 *              https://github.com/aspnet/Microsoft.Data.Sqlite
 *              http://sqlitebrowser.org/
 *              http://www.sqlite.org/cli.html#querying_the_database_schema
 *****************************************************************************/

namespace DotNetCoreBootstrap.DatabaseDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the Entity Framework for Sqlite in-memory demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages:
    /// Microsoft.EntityFrameworkCore.Sqlite
    /// Microsoft.EntityFrameworkCore.InMemory
    /// Microsoft.Data.Sqlite
    /// </remarks>
    internal static class EntityFrameworkSqliteInMemoryDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            // create entities to be inserted.
            DemoNestedEntity nestedEntity =
                new DemoNestedEntity { Name = "nested_entity_1" };
            List<DemoEntity> subEntities = new List<DemoEntity>
            {
                new DemoEntity { SubName = "sub_entity_1", ParentEntity = nestedEntity },
                new DemoEntity { SubName = "sub_entity_2", ParentEntity = nestedEntity },
                new DemoEntity { SubName = "sub_entity_3", ParentEntity = nestedEntity },
            };
            nestedEntity.SubEntities = subEntities;

            // sqlite in-memory database only exists while the connection opening.
            SqliteConnection sqliteConnection =
                new SqliteConnection("DataSource=:memory:");
            sqliteConnection.Open();

            // create the database context options for using sqlite in-memory database.
            DbContextOptions<DemoContext> options =
                new DbContextOptionsBuilder<DemoContext>()
                    .UseSqlite(sqliteConnection)
                    .Options;

            try
            {
                using (DemoContext db = new DemoContext(options))
                {
                    Console.WriteLine($"Is In-Memory database: {db.Database.IsInMemory()}");
                    Console.WriteLine($"Is IsSqlite database: {db.Database.IsSqlite()}");

                    // create database schema
                    db.Database.EnsureCreated();

                    // insert entities and save changes to database.
                    db.NestedEntities.Add(nestedEntity);
                    int count = db.SaveChanges();
                    Console.WriteLine($"{count} records saved to database");

                    // query all nested entities data.
                    Console.WriteLine();
                    Console.WriteLine("All nested entities in database:");
                    foreach (DemoNestedEntity entity in db.NestedEntities)
                    {
                        Console.WriteLine($" - id: '{entity.Id}', name '{entity.Name}'");
                    }

                    // query all sub entities data.
                    Console.WriteLine();
                    Console.WriteLine("All sub entities in database:");
                    foreach (DemoEntity entity in db.Entities)
                    {
                        Console.WriteLine($" - id: '{entity.SubId}', name '{entity.SubName}'");
                    }
                }

                // print all sqlite database tables:
                PrintAllTables(sqliteConnection);
            }
            finally
            {
                // close sqlite connection.
                if (sqliteConnection.State != ConnectionState.Closed)
                {
                    sqliteConnection.Close();
                }
            }

            // the previous sqlite in-memory datbase will be destroyed after connection close.
            try
            {
                sqliteConnection.Open();
                PrintAllTables(sqliteConnection);
            }
            finally
            {
                // close sqlite connection.
                if (sqliteConnection.State != ConnectionState.Closed)
                {
                    sqliteConnection.Close();
                }
            }
        }

        /// <summary>
        /// Print all sqlite tables.
        /// </summary>
        /// <param name="connection">The Sqlite connection.</param>
        private static void PrintAllTables(SqliteConnection connection)
        {
            Console.WriteLine();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT name FROM sqlite_master WHERE type='table'";
            Console.WriteLine($"[trace] {command.CommandText}");

            SqliteDataReader reader = command.ExecuteReader();

            List<string> tableNames = new List<string>();
            while (reader.Read())
            {
                string tableName = reader.GetString(0);
                tableNames.Add(tableName);
                Console.WriteLine($" - table name: {tableName}");
            }
            Console.WriteLine();

            foreach (string tableName in tableNames)
            {
                command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {tableName}";
                Console.WriteLine($"[trace] {command.CommandText}");

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    IEnumerable<string> fields = new int[reader.FieldCount]
                        .Select(
                            (v, i) =>
                            $"'{reader.GetName(i)}' = '{reader.GetValue(i)}'");
                    Console.WriteLine(string.Join(",", fields));
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Defines the demo database context.
        /// </summary>
        public class DemoContext : DbContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DemoContext"/> class.
            /// </summary>
            /// <param name="options">
            /// The database context options.
            /// </param>
            public DemoContext(DbContextOptions<DemoContext> options)
                : base(options)
            { }

            /// <summary>
            /// Gets or Sets the nested entities dataset.
            /// </summary>
            /// <returns>The nested entities dataset.</returns>
            public DbSet<DemoNestedEntity> NestedEntities { get; set; }

            /// <summary>
            /// Gets or sets the sub entities dataset.
            /// </summary>
            /// <returns>The sub entities dataset.</returns>
            public DbSet<DemoEntity> Entities { get; set; }

            /// <summary>
            /// Override this method to configure the database (and other options)
            /// to be used for this context.
            /// This method is called for each instance of the context that is created.
            /// </summary>
            /// <param name="optionsBuilder">
            /// A builder used to create or modify options for this context.
            /// Databases (and other extensions) typically define extension methods
            /// on this object that allow you to configure the context.
            /// </param>
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                // the IsConfigured flag will be true while received the options parameter from constructor.
                if (!optionsBuilder.IsConfigured)
                {
                    // use in-memory database by default.
                    optionsBuilder.UseInMemoryDatabase(databaseName: "in_memory_database");
                }
            }
        }

        /// <summary>
        /// Defines the nested entities class, mapping to NestedEntities table.
        /// </summary>
        [Table("nested_entities")]
        public class DemoNestedEntity
        {
            /// <summary>
            /// Gets or sets the nested entity identifier, mapping to id column,
            /// this column is primary key and the value is auto generated identity.
            /// </summary>
            /// <returns>The nested entity identifier.</returns>
            [Column("id")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the nested entity name, mapping to name column,
            /// this column is mandatory.
            /// </summary>
            /// <returns>The nested entity name.</returns>
            [Column("name")]
            [Required]
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the sub entities collection.
            /// </summary>
            /// <returns>The sub entities collection.</returns>
            public List<DemoEntity> SubEntities { get; set; }
        }

        /// <summary>
        /// Defines the sub entity class, mapping to Entities table.
        /// </summary>
        [Table("entities")]
        public class DemoEntity
        {
            /// <summary>
            /// Gets or sets the sub entity identifier, mapping to id column,
            /// this column is primary key and the value is auto generated identity.
            /// </summary>
            /// <returns>The sub entity identifier.</returns>
            [Column("id")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int SubId { get; set; }

            /// <summary>
            /// Gets or sets the sub entity name, mapping to name column,
            /// this column is mandatory.
            /// </summary>
            /// <returns>The sub entity name.</returns>
            [Column("name")]
            [Required]
            public string SubName { get; set; }

            /// <summary>
            /// Gets or sets the parent entity, this column is a foreign key.
            /// </summary>
            /// <returns>The parent entity.</returns>
            [ForeignKey("parent_id")]
            public DemoNestedEntity ParentEntity { get; set; }
        }
    }
}