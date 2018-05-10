
namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class CommandLineArgumentParserTest : CommandLineTestBase
    {
        public CommandLineArgumentParserTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public void ParseGivenNullArgsSuccessTest()
        {
            CommandLineArgument expectedValue = new CommandLineArgument();

            this.TestAssert(
                () => CommandLineArgumentParser.Parse(null),
                actualValue =>
                {
                    Assert.Equal(
                        expectedValue,
                        actualValue,
                        new CommandLineArgumentComparer());
                });
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
                this.ParseActionParams(actionParamsString);
            CommandLineArgument expectedValue =
                new CommandLineArgument(category, action, actionParams);
            string[] args = this.BuildUpArgs(category, action, actionParams);

            this.TestAssert(
                () => CommandLineArgumentParser.Parse(args),
                actualValue =>
                {
                    Assert.Equal(
                        expectedValue,
                        actualValue,
                        new CommandLineArgumentComparer());
                });
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
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.CommandLineArgsParseFailed;
            Dictionary<string, string> actionParams =
                this.ParseActionParams(actionParamsString);
            string[] args = this.BuildUpArgs(category, action, actionParams);
            string expectedMessage =
                ExceptionMessages.InvalidCommandLineArguments
                    .FormatInvariant(
                        "no_dash_prefix_param2",
                        string.Join(" ", args));

            this.AssertCommandLineException(
                expectedErrorCode,
                expectedMessage,
                () => CommandLineArgumentParser.Parse(args));
        }

        private string[] BuildUpArgs(
            string category,
            string action,
            Dictionary<string, string> actionParams)
        {
            string[] args = new List<string>
                {
                    category,
                    action
                }.Union(actionParams
                    .SelectMany(kvp => new[] { kvp.Key, kvp.Value }))
                    .Where(s => s != null)
                .ToArray();

            this.Output.WriteLine($"args: {string.Join(" ", args)}");

            return args;
        }

        private sealed class CommandLineArgumentComparer :
            IEqualityComparer<CommandLineArgument>
        {
            public bool Equals(CommandLineArgument x, CommandLineArgument y)
                => Convert.ToString(x).Equals(Convert.ToString(y));

            public int GetHashCode(CommandLineArgument obj)
                => obj.ToString().GetHashCode();
        }
    }
}
