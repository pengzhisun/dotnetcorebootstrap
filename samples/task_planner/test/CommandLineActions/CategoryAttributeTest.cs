namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;

    using Xunit;
    using Xunit.Abstractions;

    public sealed class CategoryAttributeTest
    {
        private readonly ITestOutputHelper output;

        public CategoryAttributeTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("General")]
        [InlineData("Category with whitespace")]
        public void ConstructorGivenEnumActionSuccessTest(string category)
        {
            Type actionTypeType = typeof(DummyActionEnum);

            CategoryAttribute actualValue =
                new CategoryAttribute(category, actionTypeType);
            this.output.WriteLine($"actual value: {actualValue}");

            Assert.NotNull(actualValue);
            Assert.Equal(category, actualValue.Category);
            Assert.Equal(actionTypeType, actualValue.ActionTypeType);
        }

        [Theory]
        [InlineData(null, typeof(DummyActionEnum))]
        [InlineData(null, typeof(string))]
        [InlineData(null, null)]
        public void ConstructorGivenNullCategoryFailedTest(
            string category, Type actionTypeType)
        {
            Assert.Throws<ArgumentNullException>(()=>
            {
                try
                {
                    new CategoryAttribute(category, actionTypeType);
                }
                catch (ArgumentNullException ex)
                {
                    Assert.Equal(nameof(category), ex.ParamName);
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        [Theory]
        [InlineData("", typeof(DummyActionEnum))]
        [InlineData(" ", typeof(string))]
        [InlineData("    ", null)]
        public void ConstructorGivenEmptyOrWhitespaceCategoryFailedTest(
            string category, Type actionTypeType)
        {
            Assert.Throws<ArgumentException>(()=>
            {
                try
                {
                    new CategoryAttribute(category, actionTypeType);
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(nameof(category), ex.ParamName);
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        [Fact]
        public void ConstructorGivenNullActionTypeTypeFailedTest()
        {
            Assert.Throws<ArgumentNullException>(()=>
            {
                Type actionTypeType = null;
                try
                {
                    new CategoryAttribute("Dummy", actionTypeType);
                }
                catch (ArgumentNullException ex)
                {
                    Assert.Equal(nameof(actionTypeType), ex.ParamName);
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(bool))]
        public void ConstructorGivenNonEnumActionTypeTypeFailedTest(
            Type actionTypeType)
        {
            Assert.Throws<ArgumentException>(()=>
            {
                try
                {
                    new CategoryAttribute("Dummy", actionTypeType);
                }
                catch (ArgumentException ex)
                {
                    Assert.Equal(nameof(actionTypeType), ex.ParamName);
                    this.output.PrintException(ex);
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