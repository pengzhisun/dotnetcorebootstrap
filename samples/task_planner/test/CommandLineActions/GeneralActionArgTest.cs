namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GeneralActionArgTest
    {
        private readonly ITestOutputHelper output;

        public GeneralActionArgTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("-h", null)]
        [InlineData("-h", "true")]
        [InlineData("--help", null)]
        [InlineData("--help", "true")]
        public void ConstructorGivenHelpArgSuccessTest(params string[] args)
        {
            CommandLineArgument commandLineArg = GetCommandLineArg(args);

            GeneralActionArg actualValue = new GeneralActionArg(commandLineArg);
            this.output.WriteLine($"actual value: {actualValue}");

            Assert.NotNull(actualValue);
            Assert.IsAssignableFrom<CommandLineArgument>(actualValue);
            Assert.Equal(commandLineArg.Category, actualValue.Category);
            Assert.Equal(commandLineArg.Action, actualValue.Action);
            Assert.Equal(
                commandLineArg.ActionParameters,
                actualValue.ActionParameters);
            Assert.True(actualValue.HelpSwtichEnabled);
            Assert.False(actualValue.VersionSwtichEnabled);
            Assert.True(actualValue.IsValid());
        }

        [Theory]
        [InlineData("-v", null)]
        [InlineData("-v", "true")]
        [InlineData("--version", null)]
        [InlineData("--version", "true")]
        public void ConstructorGivenVersionArgSuccessTest(params string[] args)
        {
            CommandLineArgument commandLineArg = GetCommandLineArg(args);

            GeneralActionArg actualValue = new GeneralActionArg(commandLineArg);
            this.output.WriteLine($"actual value: {actualValue}");

            Assert.NotNull(actualValue);
            Assert.IsAssignableFrom<CommandLineArgument>(actualValue);
            Assert.Equal(commandLineArg.Category, actualValue.Category);
            Assert.Equal(commandLineArg.Action, actualValue.Action);
            Assert.Equal(
                commandLineArg.ActionParameters,
                actualValue.ActionParameters);
            Assert.False(actualValue.HelpSwtichEnabled);
            Assert.True(actualValue.VersionSwtichEnabled);
            Assert.True(actualValue.IsValid());
        }

        [Theory]
        [InlineData("-h", null, "-v", null)]
        [InlineData("-h", "true", "-v", "true")]
        [InlineData("-h", "false", "-v", "false")]
        [InlineData("--help", null, "--version", null)]
        [InlineData("--help", "true", "--version", "true")]
        [InlineData("--help", "false", "--version", "false")]
        public void ConstructorGivenInvalidArgSuccessTest(params string[] args)
        {
            CommandLineArgument commandLineArg = GetCommandLineArg(args);

            GeneralActionArg actualValue = new GeneralActionArg(commandLineArg);
            this.output.WriteLine($"actual value: {actualValue}");

            Assert.False(actualValue.IsValid());
        }

        private static CommandLineArgument GetCommandLineArg(string[] args)
        {
            Dictionary<string, string> actionParams =
                new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i += 2)
            {
                actionParams[args[i]] = args[i + 1];
            }

            return new CommandLineArgument(
                CommandLineArgument.DefaultAction,
                GeneralActionType.Default.ToString(),
                actionParams);
        }
    }
}