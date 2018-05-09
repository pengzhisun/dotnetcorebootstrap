namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    internal sealed class GeneralActionArg : CommandLineArgument
    {
        [ActionParameter(false, "-h", "--help")]
        internal bool? HelpSwitch { get; set; }

        public bool HelpSwtichEnabled => this.HelpSwitch ?? false;

        [ActionParameter(false, "-v", "--version")]
        internal bool? VersionSwith { get; set; }

        public bool VersionSwtichEnabled => this.VersionSwith ?? false;

        public override bool IsValid() =>
            this.HelpSwtichEnabled ^ this.VersionSwtichEnabled;
    }
}