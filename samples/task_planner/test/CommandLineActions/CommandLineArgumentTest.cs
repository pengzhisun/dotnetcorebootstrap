namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Xunit;
    using Xunit.Abstractions;

    public sealed class CommandLineArgumentTest : CommandLineTestBase
    {
        public CommandLineArgumentTest(ITestOutputHelper output)
            : base(output)
        {
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
            Dictionary<string, string> actionParams = this.GetActionParams(args);

            this.AssertActualValue(
                () => new CommandLineArgument(category, action, actionParams),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.Equal(expectedCategory, actualValue.Category);
                    Assert.Equal(expectedAction, actualValue.Action);
                    Assert.Equal(actionParams, actualValue.ActionParameters);
                    Assert.True(actualValue.IsValid());
                });
        }

        [Fact]
        public void ConstructorForNoActionParamAttrArgSuccessTest()
        {
            CommandLineArgument commandLineArg = this.GetDummyCommandLineArg();

            this.AssertActualValue(
                () => new NoActionParamAttrArg(commandLineArg),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.IsAssignableFrom<CommandLineArgument>(actualValue);
                    Assert.Equal(commandLineArg.Category, actualValue.Category);
                    Assert.Equal(commandLineArg.Action, actualValue.Action);
                    Assert.Equal(
                        commandLineArg.ActionParameters,
                        actualValue.ActionParameters);
                    Assert.True(actualValue.IsValid());
                    Assert.Null(actualValue.DummyParam);
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
                this.GetDummyCommandLineArg(
                    new Dictionary<string, string>
                    {
                        { "-dummy-int-param", $"{expectedDummyIntParam}" },
                        { "-dummy-string-param", expectedDummyStringParam },
                        { "-dummy-bool-param", $"{expectedDummyBoolParam}" },
                        { "-dummy-nullable-int-param", $"{expectedDummyNullableIntParam}" },
                        { "-dummy-nullable-bool-false-param", $"{expectedDummyNullableBoolFalseParam}" },
                    });

            this.AssertActualValue(
                () => new SupportedParamTypesArg(commandLineArg),
                actualValue =>
                {
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
                });
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
                this.GetDummyCommandLineArg(
                    new Dictionary<string, string>
                    {
                        { "-dummy-nullable-bool-null-param", null },
                    });

            this.AssertActualValue(
                () => new DefaultValueArg(commandLineArg),
                actualValue =>
                {
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
                });
        }

        [Fact]
        public void ConstructorForMoreThanOneAliasesArgFailedTest()
        {
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.CommandLineArgInitFailed;
            string expectedMessage =
                ExceptionMessages.PropMatchedMoreThanOneActionParams
                    .FormatInvariant(nameof(MoreThanOneAliasesArg.DupDummyParam));
            CommandLineArgument commandLineArg =
                this.GetDummyCommandLineArg(
                    new Dictionary<string, string>
                    {
                        { "--dummy-param", "dummy_value" },
                        { "--dup-dummy-param", "dummy_value" }
                    });

            this.AssertCommandLineException(
                expectedErrorCode,
                expectedMessage,
                () => new MoreThanOneAliasesArg(commandLineArg));
        }

        [Fact]
        public void ConstructorForNotFoundParamValueArgFailedTest()
        {
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.CommandLineArgInitFailed;
            string expectedMessage =
                ExceptionMessages.PropMatchedActionParamValueNotNull
                    .FormatInvariant(nameof(NotFoundParamValueArg.NotFoundParam));
            CommandLineArgument commandLineArg =
                this.GetDummyCommandLineArg(
                    new Dictionary<string, string>
                    {
                        { "--not-found-value-param", null }
                    });

            this.AssertCommandLineException(
                expectedErrorCode,
                expectedMessage,
                () => new NotFoundParamValueArg(commandLineArg));
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