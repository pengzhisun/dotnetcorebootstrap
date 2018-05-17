namespace DotNetCoreBootstrap.Samples.TaskPlanner.Tasks
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    internal static class TaskDbRepository
    {
        private static readonly string DatabaseFileName =
            Path.Combine(
                AppContext.BaseDirectory,
                $"data{Path.DirectorySeparatorChar}tasks.db");

        public static TaskDbEntity CreateTask(TaskNewActionArgument arg)
        {
            return RunDbFunc(
                func: db =>
                {
                    if (db.TaskEntities.Any(e => e.TaskName.Equals(arg.Name)))
                    {
                        throw new InvalidOperationException($"Task name already existed: '{arg.Name}'.");
                    }

                    TaskDbEntity parentEntity = null;

                    if (arg.ParentId != null)
                    {
                        parentEntity =
                            db.TaskEntities.FirstOrDefault(
                                e => e.TaskId.Equals(arg.ParentId.Value));

                        if (parentEntity == null)
                        {
                            throw new InvalidOperationException($"Parent task id '{arg.ParentId}' not found.");
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(arg.ParentName))
                    {
                        parentEntity =
                            db.TaskEntities.FirstOrDefault(
                                e => e.TaskName.Equals(arg.ParentName));

                        if (parentEntity == null)
                        {
                            throw new InvalidOperationException($"Parent task name '{arg.ParentName}' not found.");
                        }
                    }

                    TaskDbEntity entity = new TaskDbEntity
                    {
                        TaskName = arg.Name,
                        TaskDescription = arg.Description,
                        ParentTask = parentEntity,
                    };

                    db.TaskEntities.Add(entity);
                    db.SaveChanges();

                    return entity;
                },
                isInMemory: arg.InMemorySwitchEnabled);
        }

        private static TResult RunDbFunc<TResult>(
            Func<TaskDbContext, TResult> func,
            bool isInMemory = false)
        {
            SqliteConnection sqliteConnection = null;
            DbContextOptions<TaskDbContext> options = null;

            if (isInMemory)
            {
                sqliteConnection =
                    new SqliteConnection("DataSource=:memory:");
                sqliteConnection.Open();

                options =
                    new DbContextOptionsBuilder<TaskDbContext>()
                        .UseSqlite(sqliteConnection)
                        .Options;
            }
            else
            {
                options =
                    new DbContextOptionsBuilder<TaskDbContext>()
                        .UseSqlite($"DataSource={DatabaseFileName}")
                        .Options;
            }

            try
            {
                using (TaskDbContext db = new TaskDbContext(options))
                {
                    if (isInMemory)
                    {
                        db.Database.EnsureCreated();
                    }

                    return func.Invoke(db);
                }
            }
            finally
            {
                if (sqliteConnection != null
                    && sqliteConnection.State != ConnectionState.Closed)
                {
                    sqliteConnection.Close();
                }
            }
        }
    }
}