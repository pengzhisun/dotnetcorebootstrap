namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Xunit;
    using Xunit.Abstractions;

    public sealed class CommandLineArgumentTest
    {
        private readonly ITestOutputHelper output;

        public CommandLineArgumentTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(null, null, "-h", null)]
        [InlineData("Dummy", null, "-param", "value")]
        [InlineData("Dummy", null, "--version", null)]
        public void ConstructorGivenNullValueSuccessTest(
            string category,
            string action,
            params string[] args)
        {
            string expectedCategory =
                category ?? CommandLineArgument.DefaultCategory;

            string expectedAction =
                action ?? CommandLineArgument.DefaultAction;

            Dictionary<string, string> actionParams =
                GetActionParams(args);

            CommandLineArgument actualValue =
                new CommandLineArgument(category, action, actionParams);

            this.output.WriteLine($"actual value: {actualValue}");

            Assert.NotNull(actualValue);
            Assert.Equal(expectedCategory, actualValue.Category);
            Assert.Equal(expectedAction, actualValue.Action);
            Assert.Equal(actionParams, actualValue.ActionParameters);
            Assert.True(actualValue.IsValid());
        }

        [Fact]
        public void ConstructorForNoActionParamAttrArgSuccessTest()
        {
            CommandLineArgument commandLineArg =
                GetDummyCommandLineArg();

            NoActionParamAttrArg actualValue =
                new NoActionParamAttrArg(commandLineArg);

            this.output.WriteLine($"actual value: {actualValue}");

            Assert.NotNull(actualValue);
            Assert.IsAssignableFrom<CommandLineArgument>(actualValue);
            Assert.Equal(commandLineArg.Category, actualValue.Category);
            Assert.Equal(commandLineArg.Action, actualValue.Action);
            Assert.Equal(
                commandLineArg.ActionParameters,
                actualValue.ActionParameters);
            Assert.True(actualValue.IsValid());
            Assert.Null(actualValue.DummyParam);
        }

        [Fact]
        public void ConstructorForMoreThanOneAliasesArgFailedTest()
        {
            CommandLineArgument commandLineArg =
                GetDummyCommandLineArg(new Dictionary<string, string>
                    {
                        { "--dummy-param", "dummy_value" },
                        { "--dup-dummy-param", "dummy_value" }
                    });

            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    new MoreThanOneAliasesArg(commandLineArg);
                }
                catch (InvalidOperationException ex)
                {
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        [Fact]
        public void ConstructorForSupportedParamTypesArgSuccessTest()
        {
            int expectedDummyIntParam = 111;
            string expectedDummyStringParam = @"dummy_value";
            bool expectedDummyBoolParam = true;
            int? expectedDummyNullableIntParam = 222;
            bool? expectedDummyNullableBoolFalseParam = false;

            CommandLineArgument commandLineArg =
                GetDummyCommandLineArg(new Dictionary<string, string>
                    {
                        { "-dummy-int-param", $"{expectedDummyIntParam}" },
                        { "-dummy-string-param", expectedDummyStringParam },
                        { "-dummy-bool-param", $"{expectedDummyBoolParam}" },
                        { "-dummy-nullable-int-param", $"{expectedDummyNullableIntParam}" },
                        { "-dummy-nullable-bool-false-param", $"{expectedDummyNullableBoolFalseParam}" },
                    });

            SupportedParamTypesArg actualValue =
                new SupportedParamTypesArg(commandLineArg);

            this.output.WriteLine($"actual value: {actualValue}");

            Assert.NotNull(actualValue);
            Assert.IsAssignableFrom<CommandLineArgument>(actualValue);
            Assert.Equal(commandLineArg.Category, actualValue.Category);
            Assert.Equal(commandLineArg.Action, actualValue.Action);
            Assert.Equal(
                commandLineArg.ActionParameters,
                actualValue.ActionParameters);
            Assert.True(actualValue.IsValid());

            Assert.Equal(
                expectedDummyIntParam,
                actualValue.DummyIntParam);

            Assert.Equal(
                expectedDummyBoolParam,
                actualValue.DummyBoolParam);

            Assert.Equal(
                expectedDummyStringParam,
                actualValue.DummyStringParam);

            Assert.Equal(
                expectedDummyNullableIntParam,
                actualValue.DummyNullableIntParam);

            Assert.Equal(
                expectedDummyNullableBoolFalseParam,
                actualValue.DummyNullableBoolFalseParam);
        }

        [Fact]
        public void ConstructorForDefaultValueArgSuccessTest()
        {
            bool? expectedDummyNullableBoolNullParam = true;
            string expectedDummyDefaultStringParam =
                DefaultValueArg.DefaultStringValue;
            int expectedDummyDefaultIntParam =
                DefaultValueArg.DefaultIntValue;

            CommandLineArgument commandLineArg =
                GetDummyCommandLineArg(new Dictionary<string, string>
                    {
                        { "-dummy-nullable-bool-null-param", null },
                    });

            DefaultValueArg actualValue =
                new DefaultValueArg(commandLineArg);

            this.output.WriteLine($"actual value: {actualValue}");

            Assert.NotNull(actualValue);
            Assert.IsAssignableFrom<CommandLineArgument>(actualValue);
            Assert.Equal(commandLineArg.Category, actualValue.Category);
            Assert.Equal(commandLineArg.Action, actualValue.Action);
            Assert.Equal(
                commandLineArg.ActionParameters,
                actualValue.ActionParameters);
            Assert.True(actualValue.IsValid());

            Assert.Equal(
                expectedDummyNullableBoolNullParam,
                actualValue.DummyNullableBoolNullParam);

            Assert.Equal(
                expectedDummyDefaultStringParam,
                actualValue.DummyDefaultStringParam);

            Assert.Equal(
                expectedDummyDefaultIntParam,
                actualValue.DummyDefaultIntParam);
        }

        [Fact]
        public void ConstructorForNotFoundParamValueArgFailedTest()
        {
            CommandLineArgument commandLineArg =
                GetDummyCommandLineArg(new Dictionary<string, string>
                    {
                        { "--not-found-value-param", null }
                    });

            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    new NotFoundParamValueArg(commandLineArg);
                }
                catch (InvalidOperationException ex)
                {
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        private static CommandLineArgument GetDummyCommandLineArg(
            Dictionary<string, string> actionParams = null)
            => new CommandLineArgument(
                "DummyCategory",
                "DummyAction",
                actionParams ?? new Dictionary<string, string>
                    {
                        { "--dummy-param", "dummy_value" }
                    });

        private static Dictionary<string, string> GetActionParams(string[] args)
        {
            Dictionary<string, string> actionParams =
                new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i += 2)
            {
                actionParams[args[i]] = args[i + 1];
            }

            return actionParams;
        }

        private sealed class NotFoundParamValueArg
            : CommandLineArgument
        {
            public NotFoundParamValueArg(CommandLineArgument arg)
                : base(arg)
            {
            }

            [ActionParameter(null, "--not-found-value-param")]
            internal string NotFoundParam { get; set; }
        }

        private sealed class DefaultValueArg
            : CommandLineArgument
        {
            public const string DefaultStringValue = "default_string_value";

            public const int DefaultIntValue = 333;

            public DefaultValueArg(CommandLineArgument arg)
                : base(arg)
            {
            }

            [ActionParameter(null, "-dummy-nullable-bool-null-param")]
            internal bool? DummyNullableBoolNullParam { get; set; }

            [ActionParameter(DefaultStringValue, "-dummy-default-string-param")]
            internal string DummyDefaultStringParam { get; set; }

            [ActionParameter(DefaultIntValue, "-dummy-default-int-param")]
            internal int DummyDefaultIntParam { get; set; }
        }

        private sealed class SupportedParamTypesArg
            : CommandLineArgument
        {
            public SupportedParamTypesArg(CommandLineArgument arg)
                : base(arg)
            {
            }

            [ActionParameter(null, "-dummy-int-param")]
            internal int DummyIntParam { get; set; }

            [ActionParameter(null, "-dummy-string-param")]
            internal string DummyStringParam { get; set; }

            [ActionParameter(null, "-dummy-bool-param")]
            internal bool DummyBoolParam { get; set; }

            [ActionParameter(null, "-dummy-nullable-int-param")]
            internal int? DummyNullableIntParam { get; set; }

            [ActionParameter(null, "-dummy-nullable-bool-false-param")]
            internal bool? DummyNullableBoolFalseParam { get; set; }
        }

        private sealed class MoreThanOneAliasesArg
            : CommandLineArgument
        {
            public MoreThanOneAliasesArg(CommandLineArgument arg)
                : base(arg)
            {
            }

            [ActionParameter(null, "--dummy-param", "--dup-dummy-param")]
            internal string DupDummyParam { get; set; }
        }

        private sealed class NoActionParamAttrArg
            : CommandLineArgument
        {
            public NoActionParamAttrArg(CommandLineArgument arg)
                : base(arg)
            {
            }

            internal string DummyParam { get; set; }
        }
    }
}