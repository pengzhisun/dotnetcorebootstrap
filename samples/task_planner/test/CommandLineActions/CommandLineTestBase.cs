namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Abstractions;

    public abstract class CommandLineTestBase : TestBase
    {
        protected CommandLineTestBase(ITestOutputHelper output)
            : base(output)
        {
        }

        internal CommandLineArgument GetDummyCommandLineArg(
            Dictionary<string, string> actionParams = null)
            => new CommandLineArgument(
                "DummyCategory",
                "DummyAction",
                actionParams ?? new Dictionary<string, string>
                    {
                        { "--dummy-param", "dummy_value" }
                    });

        protected Dictionary<string, string> GetActionParams(string[] args)
        {
            Dictionary<string, string> actionParams =
                new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i += 2)
            {
                actionParams[args[i]] = args[i + 1];
            }

            return actionParams;
        }

        protected void AssertCommandLineException(
            CommandLineErrorCode expectedErrorCode,
            string expectedMessage,
            Action testAction)
            => this.AssertException<CommandLineException>(
                testAction,
                ex =>
                {
                    Assert.Equal(expectedErrorCode, ex.ErrorCode);
                    Assert.Equal(expectedMessage, ex.Message);
                });
    }
}