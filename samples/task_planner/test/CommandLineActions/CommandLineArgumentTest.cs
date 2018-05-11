namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class CommandLineArgumentTest : CommandLineTestBase
    {
        public CommandLineArgumentTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Theory]
        [InlineData(null, null, "-h", null)]
        [InlineData("Dummy", null, "-param", "value")]
        [InlineData("Dummy", null, "--version", null)]
        public void ConstructorGivenNullValueSuccessTest(
            string category,
            string action,
            params string[] args)
        {
            string expectedCategory =
                category ?? CommandLineArgument.DefaultCategory;
            string expectedAction =
                action ?? CommandLineArgument.DefaultAction;
            Dictionary<string, string> actionParams = this.GetActionParams(args);

            this.AssertActualValue(
                () => new CommandLineArgument(category, action, actionParams),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.Equal(expectedCategory, actualValue.Category);
                    Assert.Equal(expectedAction, actualValue.Action);
                    Assert.Equal(actionParams, actualValue.ActionParameters);
                });
        }
    }
}