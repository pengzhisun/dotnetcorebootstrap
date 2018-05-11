namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GeneralActionArgumentTest : CommandLineTestBase
    {
        public GeneralActionArgumentTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Theory]
        [InlineData("-h", null)]
        [InlineData("-h", "true")]
        [InlineData("--help", null)]
        [InlineData("--help", "true")]
        public void ConstructorGivenHelpArgSuccessTest(params string[] args)
        {
            CommandLineArgument commandLineArg =
                this.GetDefaultCommandLineArg(args);

            this.AssertActualValue(
                () => new GeneralActionArgument(commandLineArg),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.IsAssignableFrom<ActionArgumentBase>(actualValue);
                    Assert.True(actualValue.HelpSwtichEnabled);
                    Assert.False(actualValue.VersionSwtichEnabled);
                    Assert.True(actualValue.IsValid());
                });
        }

        [Theory]
        [InlineData("-v", null)]
        [InlineData("-v", "true")]
        [InlineData("--version", null)]
        [InlineData("--version", "true")]
        public void ConstructorGivenVersionArgSuccessTest(params string[] args)
        {
            CommandLineArgument commandLineArg =
                this.GetDefaultCommandLineArg(args);

            this.AssertActualValue(
                () => new GeneralActionArgument(commandLineArg),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.IsAssignableFrom<ActionArgumentBase>(actualValue);
                    Assert.False(actualValue.HelpSwtichEnabled);
                    Assert.True(actualValue.VersionSwtichEnabled);
                    Assert.True(actualValue.IsValid());
                });
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
            CommandLineArgument commandLineArg =
                this.GetDefaultCommandLineArg(args);

            this.AssertActualValue(
                () => new GeneralActionArgument(commandLineArg),
                actualValue =>
                {
                    Assert.False(actualValue.IsValid());
                });
        }
    }
}