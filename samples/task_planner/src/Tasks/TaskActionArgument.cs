namespace DotNetCoreBootstrap.Samples.TaskPlanner.Tasks
{
    using DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions;

    internal class TaskActionArgument : ActionArgumentBase
    {
        public TaskActionArgument(CommandLineArgument commandLineArgument)
            : base(commandLineArgument)
        {
        }

        public bool HelpSwtichEnabled => this.HelpSwitch ?? false;

        [ActionParameter(null, "-h", "--help")]
        private bool? HelpSwitch { get; set; }

        public override bool IsValid() => this.HelpSwtichEnabled;
    }
}