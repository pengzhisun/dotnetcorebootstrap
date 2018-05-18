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
                });
        }

        public static TaskDbEntity UpdateTask(TaskSetActionArgument arg)
        {
            return RunDbFunc(
                func: db =>
                {
                    TaskDbEntity entity = null;

                    if (arg.Id != null)
                    {
                        entity =
                            db.TaskEntities.FirstOrDefault(
                                e => e.TaskId.Equals(arg.Id.Value));

                        if (entity == null)
                        {
                            throw new InvalidOperationException($"Task id '{arg.Id}' not found.");
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(arg.Name))
                    {
                        entity =
                            db.TaskEntities.FirstOrDefault(
                                e => e.TaskName.Equals(arg.Name));

                        if (entity == null)
                        {
                            throw new InvalidOperationException($"Task name '{arg.ParentName}' not found.");
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(arg.Description))
                    {
                        entity.TaskDescription = arg.Description;
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

                    if (parentEntity != null)
                    {
                        entity.ParentTask = parentEntity;
                    }

                    db.Update(entity);
                    db.SaveChanges();

                    return entity;
                });
        }

        private static TResult RunDbFunc<TResult>(
            Func<TaskDbContext, TResult> func)
        {
            DbContextOptions<TaskDbContext> options =
                new DbContextOptionsBuilder<TaskDbContext>()
                    .UseSqlite($"DataSource={DatabaseFileName}")
                    .Options;

            using (TaskDbContext db = new TaskDbContext(options))
            {
                return func.Invoke(db);
            }
        }
    }
}