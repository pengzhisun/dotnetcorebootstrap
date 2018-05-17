namespace DotNetCoreBootstrap.Samples.TaskPlanner.Tasks
{
    using DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions;

    internal class TaskNewActionArgument : TaskActionArgument
    {
        public TaskNewActionArgument(CommandLineArgument commandLineArgument)
            : base(commandLineArgument)
        {
        }

        [ActionParameter(null, "-n", "--name")]
        internal string Name { get; set; }

        [ActionParameter(null, "-d", "--description")]
        internal string Description { get; set; }

        [ActionParameter(null, "-pid", "--parent-id")]
        internal int? ParentId { get; set; }

        [ActionParameter(null, "-pn", "--parent-name")]
        internal string ParentName { get; set; }

        public override bool IsValid()
            => base.IsValid()
            || (!string.IsNullOrWhiteSpace(this.Name)
                && (this.ParentId == null || this.ParentName == null));
    }
}