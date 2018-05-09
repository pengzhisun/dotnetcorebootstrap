namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    [Category("General", typeof(GeneralActionType))]
    internal sealed class GeneralCategory
    {
        [Action(GeneralActionType.Default)]
        public void DefaultAction(GeneralActionArg arg)
        {
            if (arg.HelpSwtichEnabled)
            {
                this.ShowHelp();
            }
            else if (arg.VersionSwtichEnabled)
            {
                this.ShowVersion();
            }
            else
            {
                Console.WriteLine("Unknown state...");
                this.ShowHelp();
            }
        }

        private void ShowHelp()
        {
            Console.WriteLine("Help...");
        }

        private void ShowVersion()
        {
            Console.WriteLine("Version...");
        }
    }
}