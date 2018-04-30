/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   EntityFrameworkSqliteDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core Entity Framework for Sqlite demos.
 * Reference:   https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite
 *              https://docs.microsoft.com/en-us/ef/core/providers/sqlite/
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore
 *              https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite
 *              https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design
 *              https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools.DotNet
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
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the Entity Framework for Sqlite demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages:
    /// Microsoft.EntityFrameworkCore.Sqlite
    /// Microsoft.EntityFrameworkCore.Design
    /// Microsoft.EntityFrameworkCore.Tools.DotNet
    /// Microsoft.Data.Sqlite
    /// </remarks>
    internal static class EntityFrameworkSqliteDemo
    {
        /// <summary>
        /// Temp directory for Sqlite database file generation.
        /// </summary>
        const string TempDir = @"../EntityFrameworkSqliteDemo_temp";

        /// <summary>
        /// Sqlite demo databae file name.
        /// </summary>
        const string DatabaseFileName = @"EntityFrameworkSqliteDemo.db";

        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            // create demo database file.
            CreateDemoDatabaseFile();

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

            try
            {
                using (DemoContext db = new DemoContext())
                {
                    Console.WriteLine($"Is Sqlite database: {db.Database.IsSqlite()}");

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
            }
            finally
            {
                // print all tables in Sqlite demo database.
                PrintAllTables();

                // remove Sqlite demo database file if not in debug mode.
                if (!Debugger.IsAttached)
                {
                    File.Delete(DatabaseFileName);
                }
            }
        }

        #region Helper Methods

        /// <summary>
        /// Print all Sqlite tables.
        /// </summary>
        private static void PrintAllTables()
        {
            Console.WriteLine();
            using (SqliteConnection connection =
                new SqliteConnection($"Data Source={DatabaseFileName}"))
            {
                connection.Open();

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
        }

        /// <summary>
        /// Create Sqlite demo database file.
        /// </summary>
        private static void CreateDemoDatabaseFile()
        {
            string dataContext = typeof(DemoContext).FullName;
            List<string> commands = new List<string>
            {
                // cleanup old temp files
                $"rm -r -f ./Migrations",
                $"rm -f ./{DatabaseFileName}",
                $"rm -r -f {TempDir}",

                // create temp dir and copy files
                $"mkdir {TempDir}",
                $"cp EntityFrameworkSqliteDemo.cs {TempDir}/",
                $"cp EntityFrameworkSqliteDemo.csproj.xml {TempDir}/EntityFrameworkSqliteDemo.csproj",

                // switch working folder to temp dir
                $"pushd .",
                $"cd {TempDir}",

                // restore nuget packages
                $"dotnet restore",

                // add new migration
                $"dotnet ef migrations add InitialCreate -c {dataContext}",

                // apply migration to databae
                $"dotnet ef database update -c {dataContext}",

                // switch working folder back
                $"popd",

                // copy generated database file
                $"cp {TempDir}/{DatabaseFileName} ./",

                // remove temp dir
                $"rm -r -f {TempDir}",
            };

            RunCommands(commands);
            Console.WriteLine();
        }

        /// <summary>
        /// Run commands in a background bash process.
        /// </summary>
        /// <param name="commands">The commands to be executed.</param>
        private static void RunCommands(IEnumerable<string> commands)
        {
            // start a background bash process
            ProcessStartInfo startInfo =
                new ProcessStartInfo("bash", string.Empty)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
            Process process = new Process
            {
                StartInfo = startInfo
            };
            process.Start();

            // prepare task factory to start backgroud stream reader tasks.
            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource();
            CancellationToken cancellationToken =
                cancellationTokenSource.Token;
            TaskFactory taskFactory =
                new TaskFactory(cancellationToken);

            // manual reset flag for waiting dotnet command execution
            ManualResetEvent resetEvent = new ManualResetEvent(true);

            // background stream reader tasks collection
            List<Task> tasks = new List<Task>();
            Action<string, StreamReader> readAction =
                (prefix, reader) =>
                {
                    Task task =
                        taskFactory.StartNew(() =>
                        {
                            while (true)
                            {
                                // break the infinite loop until received cancel token
                                if (cancellationToken.IsCancellationRequested)
                                {
                                    break;
                                }

                                string msg = reader.ReadLine();
                                if (!string.IsNullOrWhiteSpace(msg))
                                {
                                    Console.WriteLine($"[{prefix}] {msg}");

                                    // set manual reset flag, unblock command execution
                                    resetEvent.Set();
                                }
                            }
                        });

                    tasks.Add(task);
                };

            readAction("stdout", process.StandardOutput);
            readAction("stderr", process.StandardError);

            foreach (string command in commands)
            {
                Console.WriteLine($"[trace] running command: {command}");
                process.StandardInput.WriteLine(command);

                // waiting dotnet command execution until manual reset flag set
                if (command.StartsWith("dotnet"))
                {
                    resetEvent.Reset();
                    resetEvent.WaitOne();
                }
            }

            // exit the bash process
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();

            // cancel all background stream reader threads
            cancellationTokenSource.Cancel(false);
            Task.WaitAll(tasks.ToArray());

            process.Dispose();
        }

        #endregion

        /// <summary>
        /// Defines the demo database context.
        /// </summary>
        public class DemoContext : DbContext
        {
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
                optionsBuilder.UseSqlite($"Data Source={DatabaseFileName}");
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