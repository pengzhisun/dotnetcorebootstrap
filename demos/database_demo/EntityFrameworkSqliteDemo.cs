namespace DotNetCoreBootstrap.DatabaseDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    public static class EntityFrameworkSqliteDemo
    {
        const string TempDir = @"../EntityFrameworkSqliteDemo_temp";
        const string DatabaseFileName = @"EntityFrameworkSqliteDemo.db";

        public static void Run()
        {
            CreateDemoDatabaseFile();

            DemoNestedEntity nestedEntity =
                new DemoNestedEntity { Name = "nested_entity_1" };
            List<DemoEntity> subEntities = new List<DemoEntity>
            {
                new DemoEntity { SubName = "sub_entity_1", ParentEntity = nestedEntity },
                new DemoEntity { SubName = "sub_entity_2", ParentEntity = nestedEntity },
                new DemoEntity { SubName = "sub_entity_3", ParentEntity = nestedEntity },
            };
            nestedEntity.SubEntities = subEntities;

            using (var db = new DemoContext())
            {
                db.NestedEntities.Add(nestedEntity);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All nested entities in database:");
                foreach (var entity in db.NestedEntities)
                {
                    Console.WriteLine($" - id: '{entity.Id}', name '{entity.Name}'");
                }
                Console.WriteLine();
                Console.WriteLine("All sub entities in database:");
                foreach (var entity in db.Entities)
                {
                    Console.WriteLine($" - id: '{entity.SubId}', name '{entity.SubName}'");
                }
            }

            File.Delete(DatabaseFileName);
        }

        private static void CreateDemoDatabaseFile()
        {
            string dataContext = typeof(DemoContext).FullName;
            List<string> commands = new List<string>
            {
                $"rm -r -f ./Migrations",
                $"rm -f ./{DatabaseFileName}",
                $"rm -r -f {TempDir}",
                $"mkdir {TempDir}",
                $"cp EntityFrameworkSqliteDemo.cs {TempDir}/",
                $"cp EntityFrameworkSqliteDemo.csprojtmp {TempDir}/EntityFrameworkSqliteDemo.csproj",
                $"pushd .",
                $"cd {TempDir}",
                $"dotnet restore",
                $"dotnet ef migrations add InitialCreate -c {dataContext}",
                "dotnet ef migrations list",
                $"dotnet ef database update -c {dataContext}",
                $"popd",
                $"cp {TempDir}/{DatabaseFileName} ./",
                $"rm -r -f {TempDir}",
            };

            RunCommands(commands);
            Console.WriteLine();
        }

        private static void RunCommands(IEnumerable<string> commands)
        {
            var startInfo = new ProcessStartInfo("bash", "")
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

            CancellationTokenSource cancellationTokenSource =
                new CancellationTokenSource();
            CancellationToken cancellationToken =
                cancellationTokenSource.Token;
            TaskFactory taskFactory =
                new TaskFactory(cancellationToken);

            List<Task> tasks = new List<Task>();
            Action<string, StreamReader> readAction =
                (prefix, reader) =>
                {
                    Task task =
                        taskFactory.StartNew(() =>
                        {
                            while (true)
                            {
                                if (cancellationToken.IsCancellationRequested)
                                {
                                    break;
                                }

                                string msg = reader.ReadLine();
                                if (!string.IsNullOrWhiteSpace(msg))
                                {
                                    Console.WriteLine($"[{prefix}] {msg}");
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
            }

            process.StandardInput.WriteLine("exit");
            process.WaitForExit();

            cancellationTokenSource.Cancel(false);
            Task.WaitAll(tasks.ToArray());
        }

        public class DemoContext : DbContext
        {
            public DbSet<DemoNestedEntity> NestedEntities { get; set; }

            public DbSet<DemoEntity> Entities { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite($"Data Source={DatabaseFileName}");
            }
        }

        [Table("NestedEntities")]
        public class DemoNestedEntity
        {
            [Column("id")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Column("name")]
            [Required]
            public string Name { get; set; }

            public List<DemoEntity> SubEntities { get; set; }
        }

        [Table("Entities")]
        public class DemoEntity
        {
            [Column("id")]
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int SubId { get; set; }

            [Column("name")]
            [Required]
            public string SubName { get; set; }

            public DemoNestedEntity ParentEntity { get; set; }
        }
    }
}