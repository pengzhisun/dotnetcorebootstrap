
namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class CommandLineEngineTest : CommandLineTestBase
    {
        public CommandLineEngineTest(ITestOutputHelper output)
            : base(output)
        {
        }

        #region Test methods for all supported commands

        [Theory]
        [InlineData("-h")]
        [InlineData("--help")]
        public void ProcessGeneralDefaultActionWithHelpParamSucessTest(
            string paramName)
        {
            CommandLineArgument arg =
                new CommandLineArgument(
                    null,
                    null,
                    new Dictionary<string, string>
                        {
                            { paramName, null }
                        });

            string expectedOut = Constants.HelpMessage + Environment.NewLine;

            this.AssertConsoleOut(
                expectedOut,
                () => new CommandLineEngine().Process(arg));
        }

        [Theory]
        [InlineData("-v")]
        [InlineData("--version")]
        public void ProcessGeneralDefaultActionWithVersionParamSucessTest(
            string paramName)
        {
            CommandLineArgument arg =
                new CommandLineArgument(
                    null,
                    null,
                    new Dictionary<string, string>
                        {
                            { paramName, null }
                        });

            Version assemblyVersion =
                Assembly.GetAssembly(typeof(CommandLineEngine)).GetName().Version;
            string expectedOut =
                string.Format(
                    CultureInfo.InvariantCulture,
                    Constants.VersionMessageFormat,
                    assemblyVersion) + Environment.NewLine;

            this.AssertConsoleOut(
                expectedOut,
                () => new CommandLineEngine().Process(arg));
        }

        #endregion

        #region Test methods for exception cases.

        [Fact]
        public void ProcessNoActionParamMethodCategoryFailedTest()
        {
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.InvalidActionMethodDefinition;
            string expectedMessage =
                ExceptionMessages.ActionMethodNotAcceptOneParam
                    .FormatInvariant(
                        typeof(NoActionParamMethodCategory)
                            .GetMethod("DummyAction"));
            CommandLineArgument arg =
                new CommandLineArgument(
                    "No_action_param_method_category",
                    DummyActionTypeEnum.DummyAction.ToString());

            this.AssertCommandLineException(
                expectedErrorCode,
                expectedMessage,
                () => DummyEngineProcess(arg));
        }

        [Fact]
        public void ProcessMoreThanOneActionParamMethodCategoryFailedTest()
        {
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.InvalidActionMethodDefinition;
            string expectedMessage =
                ExceptionMessages.ActionMethodNotAcceptOneParam
                    .FormatInvariant(
                        typeof(MoreThanOneActionParamMethodCategory)
                            .GetMethod("DummyAction"));
            CommandLineArgument arg =
                new CommandLineArgument(
                    "More_than_one_action_param_method_category",
                    DummyActionTypeEnum.DummyAction.ToString());

            this.AssertCommandLineException(
                expectedErrorCode,
                expectedMessage,
                () => DummyEngineProcess(arg));
        }

        [Fact]
        public void ProcessNoCommandLineArgParamMethodCategoryFailedTest()
        {
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.InvalidActionMethodDefinition;
            string expectedMessage =
                ExceptionMessages.ActionMethodNotAcceptOneCommandLineArgParam
                    .FormatInvariant(
                        typeof(NoCommandLineArgParamMethodCategory)
                            .GetMethod("DummyAction"));
            CommandLineArgument arg =
                new CommandLineArgument(
                    "No_comand_line_arg_param_method_category",
                    DummyActionTypeEnum.DummyAction.ToString());

            this.AssertCommandLineException(
                expectedErrorCode,
                expectedMessage,
                () => DummyEngineProcess(arg));
        }

        [Fact]
        public void ProcessNoActionMethodCategoryFailedTest()
        {
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.InvalidActionMethodDefinition;
            string expectedMessage =
                ExceptionMessages.ActionMethodNotFound
                    .FormatInvariant(DummyActionTypeEnum.DummyAction);
            CommandLineArgument arg =
                new CommandLineArgument(
                    "No_action_method_category",
                    DummyActionTypeEnum.DummyAction.ToString());

            this.AssertCommandLineException(
                expectedErrorCode,
                expectedMessage,
                () => DummyEngineProcess(arg));
        }

        [Fact]
        public void ProcessMoreThanOneActionMethodsCategoryFailedTest()
        {
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.InvalidActionMethodDefinition;
            string expectedMessage =
                ExceptionMessages.ActionMethodFoundDupDefinitions
                    .FormatInvariant(
                        DummyActionTypeEnum.DummyAction,
                        string.Join(
                            ",",
                            typeof(MoreThanOneActionMethodsCategory)
                                .GetMethods(
                                    BindingFlags.DeclaredOnly
                                    | BindingFlags.Instance
                                    | BindingFlags.Public)
                                .Select(m => m.ToString())));
            CommandLineArgument arg =
                new CommandLineArgument(
                    "More_than_one_action_methods_category",
                    DummyActionTypeEnum.DummyAction.ToString());

            this.AssertCommandLineException(
                expectedErrorCode,
                expectedMessage,
                () => DummyEngineProcess(arg));
        }

        [Fact]
        public void ProcessNotExistedCategoryFailedTest()
        {
            CommandLineArgument arg =
                new CommandLineArgument("Not_existed_category");
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.InvalidCategoryDefinition;
            string expectedMessage =
                ExceptionMessages.CategoryNotFound.FormatInvariant(arg.Category);

            this.AssertCommandLineException(
                expectedErrorCode,
                expectedMessage,
                () => DummyEngineProcess(arg));
        }

        [Fact]
        public void ProcessMoreThanOneCategoryFailedTest()
        {
            CommandLineArgument arg =
                new CommandLineArgument("More_than_one_category");
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.InvalidCategoryDefinition;
            string expectedMessage =
                ExceptionMessages.CategoryNotFoundDupDefinitions
                    .FormatInvariant(
                        arg.Category,
                        string.Join(
                            ",",
                            new[]
                            {
                                typeof(MoreThanOneCategory),
                                typeof(MoreThanOneCategoryDup)
                            }.Select(t => t.FullName)));

            this.AssertCommandLineException(
                expectedErrorCode,
                expectedMessage,
                () => DummyEngineProcess(arg));
        }

        private static void DummyEngineProcess(CommandLineArgument arg)
        {
            Assembly actionsAssembly = Assembly.GetExecutingAssembly();

            CommandLineEngine engine = new CommandLineEngine(actionsAssembly);

            engine.Process(arg);
        }

        #endregion

        #region Nested dummy category classes for exception case tests.

        [Category(
            "No_action_param_method_category",
            typeof(DummyActionTypeEnum))]
        private sealed class NoActionParamMethodCategory
        {
            [Action(DummyActionTypeEnum.DummyAction)]
            public void DummyAction()
            {
            }
        }

        [Category(
            "More_than_one_action_param_method_category",
            typeof(DummyActionTypeEnum))]
        private sealed class MoreThanOneActionParamMethodCategory
        {
            [Action(DummyActionTypeEnum.DummyAction)]
            public void DummyAction(string param1, string parm2)
            {
            }
        }

        [Category(
            "No_comand_line_arg_param_method_category",
            typeof(DummyActionTypeEnum))]
        private sealed class NoCommandLineArgParamMethodCategory
        {
            [Action(DummyActionTypeEnum.DummyAction)]
            public void DummyAction(string param1)
            {
            }
        }

        [Category("No_action_method_category", typeof(DummyActionTypeEnum))]
        private sealed class NoActionMethodCategory
        {
        }

        [Category(
            "More_than_one_action_methods_category",
            typeof(DummyActionTypeEnum))]
        private sealed class MoreThanOneActionMethodsCategory
        {
            [Action(DummyActionTypeEnum.DummyAction)]
            public void DummyAction()
            {
            }

            [Action(DummyActionTypeEnum.DummyAction)]
            public void DummyActionDup()
            {
            }
        }

        [Category("More_than_one_category", typeof(DummyActionTypeEnum))]
        private sealed class MoreThanOneCategory
        {
        }

        [Category("More_than_one_category", typeof(DummyActionTypeEnum))]
        private sealed class MoreThanOneCategoryDup
        {
        }

        private enum DummyActionTypeEnum
        {
            DummyAction
        }

        #endregion
    }
}