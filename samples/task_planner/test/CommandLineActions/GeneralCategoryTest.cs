namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GeneralCategoryTest
    {
        private readonly ITestOutputHelper output;

        public GeneralCategoryTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void DefaultConstructorSuccessTest()
        {
            GeneralCategory instance = new GeneralCategory();
            Assert.NotNull(instance);
        }

        [Theory]
        [InlineData("-h", null)]
        [InlineData("-h", "true")]
        [InlineData("--help", null)]
        [InlineData("--help", "true")]
        public void DefaultActionGivenHelpArgSuccessTest(params string[] args)
        {
            GeneralActionArg arg = GetGeneralActionArg(args);

            string expectedOut = Constants.HelpMessage + Environment.NewLine;
            using (StringWriter writer = new StringWriter())
            {
                Console.SetOut(writer);

                new GeneralCategory().DefaultAction(arg);

                writer.Flush();
                string actualOut = writer.GetStringBuilder().ToString();
                this.output.WriteLine($"actual out: {actualOut}");
                Assert.Equal(expectedOut, actualOut);
            }
        }

        [Theory]
        [InlineData("-v", null)]
        [InlineData("-v", "true")]
        [InlineData("--version", null)]
        [InlineData("--version", "true")]
        public void DefaultActionGivenVersionArgSuccessTest(params string[] args)
        {
            GeneralActionArg arg = GetGeneralActionArg(args);

            Version assemblyVersion =
                Assembly.GetAssembly(typeof(GeneralCategory)).GetName().Version;
            string expectedOut =
                string.Format(
                    CultureInfo.InvariantCulture,
                    Constants.VersionMessageFormat,
                    assemblyVersion) + Environment.NewLine;
            using (StringWriter writer = new StringWriter())
            {
                Console.SetOut(writer);

                new GeneralCategory().DefaultAction(arg);

                writer.Flush();
                string actualOut = writer.GetStringBuilder().ToString();
                this.output.WriteLine($"actual out: {actualOut}");
                Assert.Equal(expectedOut, actualOut);
            }
        }

        [Theory]
        [InlineData("-h", null, "-v", null)]
        [InlineData("-h", "true", "-v", "true")]
        [InlineData("--help", null, "--version", null)]
        [InlineData("--help", "true", "--version", "true")]
        public void DefaultActionGivenInvalidArgFailedTest(params string[] args)
        {
            GeneralActionArg arg = GetGeneralActionArg(args);

            Assert.Throws<ArgumentException>(()=>
            {
                try
                {
                    new GeneralCategory().DefaultAction(arg);
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(nameof(arg), ex.ParamName);
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        private static GeneralActionArg GetGeneralActionArg(string[] args)
        {
            Dictionary<string, string> actionParams =
                new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i += 2)
            {
                actionParams[args[i]] = args[i + 1];
            }

            return new GeneralActionArg(
                new CommandLineArgument(null, null, actionParams));
        }
    }
}