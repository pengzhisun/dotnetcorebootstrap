namespace DotNetCoreBootstrap.Samples.TaskPlanner.Tasks
{
    using Newtonsoft.Json;

    internal sealed class TaskDbEntity
    {
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public TaskDbEntity ParentTask { get; set; }

        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}