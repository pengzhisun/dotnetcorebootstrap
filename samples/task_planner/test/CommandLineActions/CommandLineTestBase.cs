namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;

    public abstract class CommandLineTestBase : TestBase
    {
        protected CommandLineTestBase(ITestOutputHelper output)
            : base(output)
        {
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

        internal CommandLineArgument GetDummyCommandLineArg(
            string category = null,
            string action = null,
            Dictionary<string, string> actionParams = null)
            => new CommandLineArgument(
                category ?? "DummyCategory",
                action ?? "DummyAction",
                actionParams ?? new Dictionary<string, string>
                    {
                        { "--dummy-param", "dummy_value" }
                    });

        internal CommandLineArgument GetDefaultCommandLineArg(
            string[] args)
            => this.GetDummyCommandLineArg(
                CommandLineArgument.DefaultCategory,
                CommandLineArgument.DefaultAction.ToString(),
                actionParams: this.GetActionParams(args));

        protected Dictionary<string, string> ParseActionParams(
            string actionParamsString)
        {
            Dictionary<string, string> actionParams;

            if (!string.IsNullOrWhiteSpace(actionParamsString))
            {
                actionParams =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(
                        actionParamsString);
            }
            else
            {
                actionParams = new Dictionary<string, string>();
            }

            return actionParams;
        }

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
    }
}