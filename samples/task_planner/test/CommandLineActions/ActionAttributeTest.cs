namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    using Xunit;
    using Xunit.Abstractions;

    public sealed class ActionAttributeTest : CommandLineTestBase
    {
        public ActionAttributeTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public void ConstructorGivenEnumActionSuccessTest()
        {
            DummyActionEnum action = DummyActionEnum.DummyAction;
            this.TestAssert(
                () => new ActionAttribute(action),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.Equal(action, actualValue.Action);
                });
        }

        [Fact]
        public void ConstructorGivenNullActionFailedTest()
        {
            string expectedParamName = @"action";

            this.AssertArgumentNullException(
                expectedParamName,
                () => new ActionAttribute(null));
        }

        [Theory]
        [InlineData("Default")]
        [InlineData(1234)]
        [InlineData(false)]
        public void ConstructorGivenNonEnumActionFailedTest(object action)
        {
            string expectedParamName = @"action";
            string expectedMessage =
                ExceptionMessages.ActionValueNotEnumValue
                    .FormatInvariant(action.GetType().Name, action)
                + $"\nParameter name: {expectedParamName}";

            this.AssertArgumentException(
                expectedParamName,
                expectedMessage,
                () => new ActionAttribute(action));
        }

        private enum DummyActionEnum
        {
            DummyAction
        }
    }
}