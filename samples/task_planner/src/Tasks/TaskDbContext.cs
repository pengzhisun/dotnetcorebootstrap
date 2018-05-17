namespace DotNetCoreBootstrap.Samples.TaskPlanner.Tasks
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal sealed class TaskDbContext : DbContext
    {
        private const string DatabaseFileName = "tasks.db";

        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options)
        {
        }

        public TaskDbContext()
        {
        }

        public DbSet<TaskDbEntity> TaskEntities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={DatabaseFileName}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskEntityTypeConfiguration());
        }

        private sealed class TaskEntityTypeConfiguration
            : IEntityTypeConfiguration<TaskDbEntity>
        {
            public void Configure(EntityTypeBuilder<TaskDbEntity> builder)
            {
                builder.ToTable("tasks")
                    .HasKey(e => e.TaskId);

                builder.Property(e => e.TaskId)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                builder.Property(e => e.TaskName)
                    .HasColumnName("name")
                    .HasMaxLength(256)
                    .IsUnicode()
                    .IsRequired();

                builder.Property(e => e.TaskDescription)
                    .HasColumnName("description")
                    .HasMaxLength(2048)
                    .IsUnicode();

                builder.Property<int?>("parent_id");

                builder.HasIndex(e => e.TaskId);

                builder.HasIndex(e => e.TaskName)
                    .IsUnique(true);

                builder.HasIndex("parent_id");

                builder.HasOne(e => e.ParentTask)
                    .WithMany()
                    .HasForeignKey("parent_id");
            }
        }
    }
}