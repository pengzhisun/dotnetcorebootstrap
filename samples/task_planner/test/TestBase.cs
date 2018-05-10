namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System;
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public abstract class TestBase
    {
        private readonly ITestOutputHelper output;

        protected TestBase(ITestOutputHelper output)
        {
            this.output = output;
        }

        protected ITestOutputHelper Output => this.output;

        protected void TestAssert<TValue>(
            Func<TValue> testAction,
            Action<TValue> assertAction)
        {
            try
            {
                TValue actualValue = testAction();
                this.output.WriteLine($"actual value: {actualValue}");
                assertAction(actualValue);
            }
            catch (XunitException)
            {
                throw;
            }
            catch (Exception ex)
            {
                this.output.PrintException(ex);
                throw;
            }
        }

        protected void AssertException<TException>(
            Action testAction,
            Action<TException> assertAction)
            where TException : Exception
        {
            Assert.Throws<TException>(()=>
            {
                try
                {
                    testAction();
                }
                catch (TException ex)
                {
                    assertAction(ex);
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        protected void AssertArgumentNullException(
            string expectedParamName,
            Action testAction)
            => this.AssertException<ArgumentNullException>(
                testAction,
                ex =>
                {
                    Assert.Equal(expectedParamName, ex.ParamName);
                });

        protected void AssertArgumentException(
            string expectedParamName,
            string expectedMessage,
            Action testAction)
            => this.AssertException<ArgumentException>(
                testAction,
                ex =>
                {
                    Assert.Equal(expectedParamName, ex.ParamName);
                    Assert.Equal(expectedMessage, ex.Message);
                });
    }
}