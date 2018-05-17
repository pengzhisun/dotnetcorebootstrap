namespace DotNetCoreBootstrap.Samples.TaskPlanner.Tasks
{
    using DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions;
    using Newtonsoft.Json;

    internal class TaskActionArgument : ActionArgumentBase
    {
        public TaskActionArgument(CommandLineArgument commandLineArgument)
            : base(commandLineArgument)
        {
        }

        public bool HelpSwtichEnabled => this.HelpSwitch ?? false;

        public bool InMemorySwitchEnabled => this.InMemorySwitch ?? false;

        [ActionParameter(null, "-h", "--help")]
        protected bool? HelpSwitch { get; set; }

        [ActionParameter(null, "-im", "--in-memory")]
        protected bool? InMemorySwitch { get; set; }

        public override bool IsValid() => this.HelpSwtichEnabled;

        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}