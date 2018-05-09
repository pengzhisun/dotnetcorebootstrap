namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    using Xunit;
    using Xunit.Abstractions;

    public sealed class ActionParameterAttributeTest
    {
        private readonly ITestOutputHelper output;

        public ActionParameterAttributeTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(null, new[]{ "-h", "--help" })]
        [InlineData("Default", new[]{ "--help" })]
        [InlineData(1234, new[]{ "-h", "--help" })]
        [InlineData(false, new[]{ "-h" })]
        public void ConstructorGivenDefaultValueAndAliasesSuccessTest(
            object defaultValue, string[] aliases)
        {
            ActionParameterAttribute actualValue =
                new ActionParameterAttribute(defaultValue, aliases: aliases);
            this.output.WriteLine($"actual value: {actualValue}");

            Assert.NotNull(actualValue);
            Assert.Equal(defaultValue, actualValue.DefaultValue);
            Assert.Equal(aliases, actualValue.Aliases);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Default")]
        [InlineData(1234)]
        [InlineData(false)]
        public void ConstructorGivenNullAliasesFailedTest(object defaultValue)
        {
            Assert.Throws<ArgumentNullException>(()=>
            {
                string[] aliases = null;
                try
                {
                    new ActionParameterAttribute(defaultValue, aliases: aliases);
                }
                catch (ArgumentNullException ex)
                {
                    Assert.Equal(nameof(aliases), ex.ParamName);
                    throw;
                }
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Default")]
        [InlineData(1234)]
        [InlineData(false)]
        public void ConstructorGivenEmptyArrayAliasesFailedTest(object defaultValue)
        {
            Assert.Throws<ArgumentException>(()=>
            {
                string[] aliases = new string[0];
                try
                {
                    new ActionParameterAttribute(defaultValue, aliases: aliases);
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(nameof(aliases), ex.ParamName);
                    throw;
                }
            });
        }
    }
}