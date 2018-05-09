
namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class CommandLineArgumentParserTests
    {
        private static readonly CommandLineArgument DefaultCommandLineArgument =
            new CommandLineArgument();

        private readonly ITestOutputHelper output;

        public CommandLineArgumentParserTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ParseGivenNullArgsSuccessTest()
        {
            CommandLineArgument expectedValue = DefaultCommandLineArgument;
            CommandLineArgument actualValue =
                CommandLineArgumentParser.Parse(null);
            this.output.WriteLine($"actual value: {actualValue}");

            Assert.Equal(
                expectedValue,
                actualValue,
                CommandLineArgumentComparer.Instance);
        }

        [Theory]
        [InlineData("dummy_category", null, null)]
        [InlineData("dummy_category", "dummy_action", null)]
        [InlineData("dummy_category", "action contains whitespace", null)]
        [InlineData("dummy_category sub_category", "dummy_action", null)]
        [InlineData("dummy_category sub_category1 sub_category2", "dummy_action", null)]
        [InlineData("dummy_category", "dummy_action", "{\"-param1\":\"value1\"}")]
        [InlineData("dummy_category", "dummy_action", "{\"-param1\":\"value contains whitespace 1\"}")]
        [InlineData("dummy_category", "dummy_action", "{\"-param1\":null,\"-param2\":null}")]
        [InlineData("dummy_category", "dummy_action", "{\"-param1\":\"value1\",\"--param-2\":\"value2\",\"-param3\":null}")]
        [InlineData("dummy_category", null, "{\"-param1\":\"value1\"}")]
        [InlineData(null, null, "{\"-param1\":\"value1\"}")]
        public void ParseGivenCategoryActionAndParamArgsSuccessTest(
            string category,
            string action,
            string actionParamsString)
        {
            Dictionary<string, string> actionParams =
                ParseActionParams(actionParamsString);

            CommandLineArgument expectedValue =
                new CommandLineArgument(category, action, actionParams);

            string[] args = BuildUpArgs(category, action, actionParams);

            this.output.WriteLine($"args: {string.Join(" ", args)}");

            CommandLineArgument actualValue =
                CommandLineArgumentParser.Parse(args);
            this.output.WriteLine($"actual value: {actualValue}");

            Assert.Equal(
                expectedValue,
                actualValue,
                CommandLineArgumentComparer.Instance);
        }

        [Theory]
        [InlineData("dummy_category", "dummy_action", "{\"-param1\":\"value1\",\"no_dash_prefix_param2\":\"value2\"}")]
        [InlineData("dummy_category", null, "{\"-param1\":\"value1\",\"no_dash_prefix_param2\":\"value2\"}")]
        [InlineData(null, null, "{\"-param1\":\"value1\",\"no_dash_prefix_param2\":\"value2\"}")]
        public void ParseGivenInvalidParamArgsFailedTest(
            string category,
            string action,
            string actionParamsString)
        {
            Dictionary<string, string> actionParams =
                ParseActionParams(actionParamsString);

            CommandLineArgument expectedValue =
                new CommandLineArgument(category, action, actionParams);

            string[] args = BuildUpArgs(category, action, actionParams);

            this.output.WriteLine($"args: {string.Join(" ", args)}");

            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    CommandLineArgumentParser.Parse(args);
                }
                catch (InvalidOperationException ex)
                {
                    this.output.WriteLine(ex.Message);
                    this.output.WriteLine(ex.StackTrace);
                    throw;
                }
            });
        }

        private static string[] BuildUpArgs(
            string category,
            string action,
            Dictionary<string, string> actionParams)
            => new List<string>
                {
                    category,
                    action
                }.Union(actionParams
                    .SelectMany(kvp => new[] { kvp.Key, kvp.Value }))
                    .Where(s => s != null)
                .ToArray();

        private static Dictionary<string, string> ParseActionParams(
            string actionParamsString)
        {
            Dictionary<string, string> actionParams;

            if (!string.IsNullOrWhiteSpace(actionParamsString))
            {
                actionParams =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(
                        actionParamsString);
            }
            else
            {
                actionParams = new Dictionary<string, string>();
            }

            return actionParams;
        }

        private sealed class CommandLineArgumentComparer :
            IEqualityComparer<CommandLineArgument>
        {
            private static readonly CommandLineArgumentComparer singleton =
                new CommandLineArgumentComparer();

            private CommandLineArgumentComparer()
            {
            }

            public static CommandLineArgumentComparer Instance
                => singleton;

            public bool Equals(CommandLineArgument x, CommandLineArgument y)
                => Convert.ToString(x).Equals(Convert.ToString(y));

            public int GetHashCode(CommandLineArgument obj)
                => obj.ToString().GetHashCode();
        }
    }
}
