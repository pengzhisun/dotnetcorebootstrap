namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    internal sealed class GeneralActionArg : CommandLineArgument
    {
        public GeneralActionArg(CommandLineArgument arg)
            : base(arg)
        {
        }

        [ActionParameter(false, "-h", "--help")]
        private bool? HelpSwitch { get; set; }

        public bool HelpSwtichEnabled => this.HelpSwitch ?? false;

        [ActionParameter(false, "-v", "--version")]
        private bool? VersionSwith { get; set; }

        public bool VersionSwtichEnabled => this.VersionSwith ?? false;

        public override bool IsValid() =>
            this.HelpSwtichEnabled ^ this.VersionSwtichEnabled;
    }
}