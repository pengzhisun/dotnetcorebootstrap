
namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class CommandLineEngineTest
    {
        private readonly ITestOutputHelper output;

        public CommandLineEngineTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("-h")]
        [InlineData("--help")]
        public void ProcessGeneralDefaultActionWithHelpParamSucessTest(
            string paramName)
        {
            CommandLineArgument arg =
                new CommandLineArgument(
                    null,
                    null,
                    new Dictionary<string, string>
                        {
                            { paramName, null }
                        });

            string expectedOut = Constants.HelpMessage + Environment.NewLine;
            using (StringWriter writer = new StringWriter())
            {
                Console.SetOut(writer);

                CommandLineEngine.Process(arg);

                writer.Flush();
                string actualOut = writer.GetStringBuilder().ToString();
                this.output.WriteLine($"actual out: {actualOut}");
                Assert.Equal(expectedOut, actualOut);
            }
        }

        [Theory]
        [InlineData("-v")]
        [InlineData("--version")]
        public void ProcessGeneralDefaultActionWithVersionParamSucessTest(
            string paramName)
        {
            CommandLineArgument arg =
                new CommandLineArgument(
                    null,
                    null,
                    new Dictionary<string, string>
                        {
                            { paramName, null }
                        });

            Version assemblyVersion =
                Assembly.GetAssembly(typeof(CommandLineEngine)).GetName().Version;
            string expectedOut =
                string.Format(
                    CultureInfo.InvariantCulture,
                    Constants.VersionMessageFormat,
                    assemblyVersion) + Environment.NewLine;
            using (StringWriter writer = new StringWriter())
            {
                Console.SetOut(writer);

                CommandLineEngine.Process(arg);

                writer.Flush();
                string actualOut = writer.GetStringBuilder().ToString();
                this.output.WriteLine($"actual out: {actualOut}");
                Assert.Equal(expectedOut, actualOut);
            }
        }
    }
}