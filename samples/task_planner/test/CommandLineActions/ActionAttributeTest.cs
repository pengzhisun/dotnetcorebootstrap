namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    using Xunit;
    using Xunit.Abstractions;

    public sealed class ActionAttributeTest
    {
        private readonly ITestOutputHelper output;

        public ActionAttributeTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ConstructorGivenEnumActionSuccessTest()
        {
            DummyActionEnum action = DummyActionEnum.DummyAction;

            ActionAttribute actualValue = new ActionAttribute(action);
            this.output.WriteLine($"actual value: {actualValue}");

            Assert.NotNull(actualValue);
            Assert.Equal(action, actualValue.Action);
        }

        [Fact]
        public void ConstructorGivenNullActionFailedTest()
        {
            Assert.Throws<ArgumentNullException>(()=>
            {
                object action = null;
                try
                {
                    new ActionAttribute(action);
                }
                catch (ArgumentNullException ex)
                {
                    Assert.Equal(nameof(action), ex.ParamName);
                    this.output.WriteLine(ex.Message);
                    this.output.WriteLine(ex.StackTrace);
                    throw;
                }
            });
        }

        [Theory]
        [InlineData("Default")]
        [InlineData(1234)]
        [InlineData(false)]
        public void ConstructorGivenNonEnumActionFailedTest(object action)
        {
            Assert.Throws<ArgumentException>(()=>
            {
                try
                {
                    new ActionAttribute(action);
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(nameof(action), ex.ParamName);
                    this.output.WriteLine(ex.Message);
                    this.output.WriteLine(ex.StackTrace);
                    throw;
                }
            });
        }

        private enum DummyActionEnum
        {
            DummyAction
        }
    }
}