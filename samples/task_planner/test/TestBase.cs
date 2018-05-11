namespace DotNetCoreBootstrap.Samples.TaskPlanner
{
    using System;
    using System.IO;
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public abstract class TestBase
    {
        private static readonly object AsssertConsoleOutLock = new object();

        private readonly ITestOutputHelper output;

        protected TestBase(ITestOutputHelper output)
        {
            this.output = output;
        }

        protected ITestOutputHelper Output => this.output;

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

        protected void AssertActualValue<TValue>(
            Func<TValue> actualValueGetter,
            Action<TValue> assertAction)
        {
            TValue actualValue = default(TValue);
            try
            {
                actualValue = actualValueGetter();
                this.output.WriteLine($"actual value: {actualValue}");
            }
            catch (Exception ex)
            {
                this.output.PrintException(ex);
                throw new XunitException(ex.GetDetail());
            }

            assertAction(actualValue);
        }

        protected void AssertConsoleOut(
            string expectedOut,
            Action testAction)
            => this.AssertActualValue(
                () =>
                {
                    string actualOut = null;

                    lock (AsssertConsoleOutLock)
                    {
                        using (StringWriter writer = new StringWriter())
                        {
                            Console.SetOut(writer);

                            testAction();

                            writer.Flush();
                            actualOut = writer.GetStringBuilder().ToString();

                            StreamWriter standardOutput =
                                new StreamWriter(Console.OpenStandardOutput());
                            standardOutput.AutoFlush = true;
                            Console.SetOut(standardOutput);
                        }
                    }

                    return actualOut;
                },
                actualValue =>
                {
                    Assert.Equal(expectedOut, actualValue);
                });
    }
}