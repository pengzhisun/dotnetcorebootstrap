namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    using Xunit;
    using Xunit.Abstractions;

    public sealed class CategoryAttributeTest : CommandLineTestBase
    {
        public CategoryAttributeTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Theory]
        [InlineData("General")]
        [InlineData("Category with whitespace")]
        public void ConstructorGivenEnumActionSuccessTest(string category)
        {
            Type actionTypeType = typeof(DummyActionEnum);

            this.AssertActualValue(
                () => new CategoryAttribute(category, actionTypeType),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.Equal(category, actualValue.Category);
                    Assert.Equal(actionTypeType, actualValue.ActionTypeType);
                });
        }

        [Theory]
        [InlineData(null, typeof(DummyActionEnum))]
        [InlineData(null, typeof(string))]
        [InlineData(null, null)]
        public void ConstructorGivenNullCategoryFailedTest(
            string category,
            Type actionTypeType)
        {
            string expectedParamName = @"category";

            this.AssertArgumentNullException(
                expectedParamName,
                () => new CategoryAttribute(category, actionTypeType));
        }

        [Theory]
        [InlineData("", typeof(DummyActionEnum))]
        [InlineData(" ", typeof(string))]
        [InlineData("    ", null)]
        public void ConstructorGivenEmptyOrWhitespaceCategoryFailedTest(
            string category, Type actionTypeType)
        {
            string expectedParamName = "category";
            string expectedMessage =
                ExceptionMessages.CategoryNotEmptyNorWhitespace
                    .FormatInvariant(category)
                + $"\nParameter name: {expectedParamName}";

            this.AssertArgumentException(
                expectedParamName,
                expectedMessage,
                () => new CategoryAttribute(category, actionTypeType));
        }

        [Fact]
        public void ConstructorGivenNullActionTypeTypeFailedTest()
        {
            string expectedParamName = @"actionTypeType";

            this.AssertArgumentNullException(
                expectedParamName,
                () => new CategoryAttribute("Dummy", null));
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(bool))]
        public void ConstructorGivenNonEnumActionTypeTypeFailedTest(
            Type actionTypeType)
        {
            string expectedParamName = @"actionTypeType";
            string expectedMessage =
                ExceptionMessages.AciontTypeTypeNotEnumType
                    .FormatInvariant(actionTypeType.Name)
                + $"\nParameter name: {expectedParamName}";

            this.AssertArgumentException(
                expectedParamName,
                expectedMessage,
                () => new CategoryAttribute("Dummy", actionTypeType));
        }

        private enum DummyActionEnum
        {
            DummyAction
        }
    }
}