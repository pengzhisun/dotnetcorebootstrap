namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    using Xunit;
    using Xunit.Abstractions;

    public sealed class ActionParameterAttributeTest : CommandLineTestBase
    {
        public ActionParameterAttributeTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Theory]
        [InlineData(null, new[]{ "-h", "--help" })]
        [InlineData("Default", new[]{ "--help" })]
        [InlineData(1234, new[]{ "-h", "--help" })]
        [InlineData(false, new[]{ "-h" })]
        public void ConstructorGivenDefaultValueAndAliasesSuccessTest(
            object defaultValue,
            string[] aliases)
        {
            this.AssertActualValue(
                () => new ActionParameterAttribute(
                    defaultValue,
                    aliases: aliases),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.Equal(defaultValue, actualValue.DefaultValue);
                    Assert.Equal(aliases, actualValue.Aliases);
                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Default")]
        [InlineData(1234)]
        [InlineData(false)]
        public void ConstructorGivenNullAliasesFailedTest(object defaultValue)
        {
            string expectedParamName = @"aliases";

            this.AssertArgumentNullException(
                expectedParamName,
                () => new ActionParameterAttribute(defaultValue, aliases: null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Default")]
        [InlineData(1234)]
        [InlineData(false)]
        public void ConstructorGivenEmptyArrayAliasesFailedTest(object defaultValue)
        {
            string expectedParamName = "aliases";
            string expectedMessage =
                ExceptionMessages.ActionParamNoAlias
                + $"\nParameter name: {expectedParamName}";

            this.AssertArgumentException(
                expectedParamName,
                expectedMessage,
                () => new ActionParameterAttribute(
                    defaultValue,
                    aliases: new string[0]));
        }
    }
}