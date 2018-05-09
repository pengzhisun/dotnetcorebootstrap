namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Reflection;

    [Category("General", typeof(GeneralActionType))]
    internal sealed class GeneralCategory
    {
        [Action(GeneralActionType.Default)]
        public void DefaultAction(GeneralActionArg arg)
        {
            if (!arg.IsValid())
            {
                throw new ArgumentException(
                    $"The argument '{arg}' is invalid",
                    nameof(arg));
            }

            if (arg.HelpSwtichEnabled)
            {
                this.ShowHelp();
            }
            else if (arg.VersionSwtichEnabled)
            {
                this.ShowVersion();
            }
        }

        private void ShowHelp()
        {
            Console.WriteLine(Constants.HelpMessage);
        }

        private void ShowVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine(Constants.VersionMessageFormat, version);
        }
    }
}