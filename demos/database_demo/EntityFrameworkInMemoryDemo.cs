/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   EntityFrameworkInMemoryDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core Entity Framework with in-memory database provider demos.
 * Reference:   https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/in-memory
 *              https://docs.microsoft.com/en-us/ef/core/providers/in-memory/
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore
 *              https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory
 *              https://github.com/aspnet/EntityFrameworkCore/tree/dev/src/EFCore.InMemory
 *****************************************************************************/

namespace DotNetCoreBootstrap.DatabaseDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the Entity Framework with in-memory database provider demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages:
    /// Microsoft.EntityFrameworkCore.InMemory
    /// Microsoft.Extensions.Logging
    /// Microsoft.Extensions.Logging.TraceSource
    /// </remarks>
    internal static class EntityFrameworkInMemoryDemo
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

            // write verbose logs to file in debug mode.
            ILoggerFactory loggerFactory = null;
            if (!Debugger.IsAttached)
            {
                string logFilePath =
                    Path.Combine(AppContext.BaseDirectory, "EntityFrameworkInMemoryDemo.log");
                Stream logStream = File.Create(logFilePath);
                TextWriterTraceListener traceListener = new TextWriterTraceListener(logStream);
                SourceSwitch verboseSwitch = new SourceSwitch("VerboseSwitch", "Verbose");
                loggerFactory =
                    new LoggerFactory().AddTraceSource(verboseSwitch, traceListener);
            }

            try
            {
                using (DemoContext db = new DemoContext(loggerFactory))
                {
                    Console.WriteLine($"Is In-Memory database: {db.Database.IsInMemory()}");

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
                // release log file handler
                loggerFactory.Dispose();
            }
        }

        /// <summary>
        /// Defines the demo database context.
        /// </summary>
        public class DemoContext : DbContext
        {
            /// <summary>
            /// The internal logger factory instance.
            /// </summary>
            private readonly ILoggerFactory loggerFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="DemoContext"/> class.
            /// </summary>
            /// <param name="loggerFactory">
            /// The logger factory instance for in-memory database logs.
            /// </param>
            public DemoContext(ILoggerFactory loggerFactory = null)
            {
                this.loggerFactory = loggerFactory;
            }

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
                if (this.loggerFactory != null)
                {
                    optionsBuilder.UseLoggerFactory(this.loggerFactory);
                }

                optionsBuilder.UseInMemoryDatabase(databaseName: "in_memory_database");
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