namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GeneralCategoryDefinitionTest : CommandLineTestBase
    {
        public GeneralCategoryDefinitionTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Theory]
        [InlineData("-h", null)]
        [InlineData("-h", "true")]
        [InlineData("--help", null)]
        [InlineData("--help", "true")]
        public void DefaultActionGivenHelpArgSuccessTest(params string[] args)
        {
            string expectedOut = Constants.HelpMessage + Environment.NewLine;
            GeneralActionArgument arg = this.GetGeneralActionArg(args);

            this.AssertConsoleOut(
                expectedOut,
                () => RunDefaultAction(arg));
        }

        [Theory]
        [InlineData("-v", null)]
        [InlineData("-v", "true")]
        [InlineData("--version", null)]
        [InlineData("--version", "true")]
        public void DefaultActionGivenVersionArgSuccessTest(params string[] args)
        {
            Version assemblyVersion =
                Assembly.GetAssembly(typeof(GeneralCategoryDefinition)).GetName().Version;
            string expectedOut =
                string.Format(
                    CultureInfo.InvariantCulture,
                    Constants.VersionMessageFormat,
                    assemblyVersion) + Environment.NewLine;
            GeneralActionArgument arg = this.GetGeneralActionArg(args);

            this.AssertConsoleOut(
                expectedOut,
                () => RunDefaultAction(arg));
        }

        [Theory]
        [InlineData("-h", null, "-v", null)]
        [InlineData("-h", "true", "-v", "true")]
        [InlineData("--help", null, "--version", null)]
        [InlineData("--help", "true", "--version", "true")]
        public void DefaultActionGivenInvalidArgSuccessTest(params string[] args)
        {
            string expectedOut = Constants.HelpMessage + Environment.NewLine;
            GeneralActionArgument arg = this.GetGeneralActionArg(args);

            this.AssertConsoleOut(
                expectedOut,
                () => RunDefaultAction(arg));
        }

        private static void RunDefaultAction(GeneralActionArgument arg)
            => GeneralCategoryDefinition.DefaultAction(arg);

        private GeneralActionArgument GetGeneralActionArg(string[] args)
            => new GeneralActionArgument(this.GetDefaultCommandLineArg(args));
    }
}