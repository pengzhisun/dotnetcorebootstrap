
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

    public sealed class CommandLineEngineTest
    {
        private readonly ITestOutputHelper output;

        public CommandLineEngineTest(ITestOutputHelper output)
        {
            this.output = output;
        }

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
            using (StringWriter writer = new StringWriter())
            {
                Console.SetOut(writer);

                new CommandLineEngine().Process(arg);

                writer.Flush();
                string actualOut = writer.GetStringBuilder().ToString();
                this.output.WriteLine($"actual out: {actualOut}");
                Assert.Equal(expectedOut, actualOut);
            }
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
            using (StringWriter writer = new StringWriter())
            {
                Console.SetOut(writer);

                new CommandLineEngine().Process(arg);

                writer.Flush();
                string actualOut = writer.GetStringBuilder().ToString();
                this.output.WriteLine($"actual out: {actualOut}");
                Assert.Equal(expectedOut, actualOut);
            }
        }

        [Fact]
        public void ProcessNoCommandLineArgParamMethodCategoryFailedTest()
        {
            CommandLineArgument arg =
                new CommandLineArgument(
                    "No_comand_line_arg_param_method_category",
                    DummyActionTypeEnum.DummyAction.ToString());

            Assembly actionsAssembly = Assembly.GetExecutingAssembly();

            CommandLineEngine engine = new CommandLineEngine(actionsAssembly);
            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    engine.Process(arg);
                }
                catch (InvalidOperationException ex)
                {
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        [Fact]
        public void ProcessMoreThanOneActionParamMethodCategoryFailedTest()
        {
            CommandLineArgument arg =
                new CommandLineArgument(
                    "More_than_one_action_param_method_category",
                    DummyActionTypeEnum.DummyAction.ToString());

            Assembly actionsAssembly = Assembly.GetExecutingAssembly();

            CommandLineEngine engine = new CommandLineEngine(actionsAssembly);
            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    engine.Process(arg);
                }
                catch (InvalidOperationException ex)
                {
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        [Fact]
        public void ProcessNoActionParamMethodCategoryFailedTest()
        {
            CommandLineArgument arg =
                new CommandLineArgument(
                    "No_action_param_method_category",
                    DummyActionTypeEnum.DummyAction.ToString());

            Assembly actionsAssembly = Assembly.GetExecutingAssembly();

            CommandLineEngine engine = new CommandLineEngine(actionsAssembly);
            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    engine.Process(arg);
                }
                catch (InvalidOperationException ex)
                {
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        [Fact]
        public void ProcessMoreThanOneActionMethodsCategoryFailedTest()
        {
            CommandLineArgument arg =
                new CommandLineArgument(
                    "More_than_one_action_methods_category",
                    DummyActionTypeEnum.DummyAction.ToString());

            Assembly actionsAssembly = Assembly.GetExecutingAssembly();

            CommandLineEngine engine = new CommandLineEngine(actionsAssembly);
            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    engine.Process(arg);
                }
                catch (InvalidOperationException ex)
                {
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        [Fact]
        public void ProcessNoActionMethodCategoryFailedTest()
        {
            CommandLineArgument arg =
                new CommandLineArgument(
                    "No_action_method_category",
                    DummyActionTypeEnum.DummyAction.ToString());

            Assembly actionsAssembly = Assembly.GetExecutingAssembly();

            CommandLineEngine engine = new CommandLineEngine(actionsAssembly);
            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    engine.Process(arg);
                }
                catch (InvalidOperationException ex)
                {
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        [Fact]
        public void ProcessNotExistedCategoryFailedTest()
        {
            CommandLineArgument arg =
                new CommandLineArgument("Not_existed_category");

            Assembly actionsAssembly = Assembly.GetExecutingAssembly();

            CommandLineEngine engine = new CommandLineEngine(actionsAssembly);
            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    engine.Process(arg);
                }
                catch (InvalidOperationException ex)
                {
                    this.output.PrintException(ex);
                    throw;
                }
            });
        }

        [Fact]
        public void ProcessMoreThanOneCategoryFailedTest()
        {
            CommandLineArgument arg =
                new CommandLineArgument("More_than_one_category");

            Assembly actionsAssembly = Assembly.GetExecutingAssembly();

            CommandLineEngine engine = new CommandLineEngine(actionsAssembly);
            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    engine.Process(arg);
                }
                catch (InvalidOperationException ex)
                {
                    this.output.PrintException(ex);
                    throw;
                }
            });
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

        [Category("No_action_method_category", typeof(DummyActionTypeEnum))]
        private sealed class NoActionMethodCategory
        {
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
    }
}