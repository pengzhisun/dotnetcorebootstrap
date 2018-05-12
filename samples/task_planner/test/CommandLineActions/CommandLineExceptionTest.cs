namespace DotNetCoreBootstrap.Samples.TaskPlanner.CommandLineActions
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class CommandLineExceptionTest : CommandLineTestBase
    {
        public CommandLineExceptionTest(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact]
        public void DefaultConstructorSuccessTest()
        {
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.Unknown;
            string expectedErrorMessage =
                ExceptionMessages.DefaultCommandLineExceptionMessage
                    .FormatInvariant(expectedErrorCode);

            this.AssertActualValue(
                () => new CommandLineException(),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.IsAssignableFrom<Exception>(actualValue);
                    Assert.Equal(expectedErrorCode, actualValue.ErrorCode);
                    Assert.Equal(expectedErrorMessage, actualValue.Message);
                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData(@"Dummy exception message.")]
        public void ConstructorGivenMessageSuccessTest(string message)
        {
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.Unknown;
            string expectedErrorMessage =
                message
                ?? ExceptionMessages.DefaultCommandLineExceptionMessage
                    .FormatInvariant(expectedErrorCode);

            this.AssertActualValue(
                () => new CommandLineException(message),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.IsAssignableFrom<Exception>(actualValue);
                    Assert.Equal(expectedErrorCode, actualValue.ErrorCode);
                    Assert.Equal(expectedErrorMessage, actualValue.Message);
                });
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(@"Dummy exception message.", null)]
        [InlineData(null, @"Inner exception message.")]
        [InlineData(@"Dummy exception message.", @"Inner exception message.")]
        public void ConstructorGivenMessageAndInnerExceptionSuccessTest(
            string message,
            string innerExceptionMessage)
        {
            CommandLineErrorCode expectedErrorCode =
                CommandLineErrorCode.Unknown;
            string expectedErrorMessage =
                message
                ?? ExceptionMessages.DefaultCommandLineExceptionMessage
                    .FormatInvariant(expectedErrorCode);
            Exception expectedInnerException =
                new Exception(innerExceptionMessage);

            this.AssertActualValue(
                () => new CommandLineException(message, expectedInnerException),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.IsAssignableFrom<Exception>(actualValue);
                    Assert.Equal(expectedErrorCode, actualValue.ErrorCode);
                    Assert.Equal(expectedErrorMessage, actualValue.Message);
                    Assert.Equal(
                        expectedInnerException,
                        actualValue.InnerException);
                });
        }

        [Theory]
        [InlineData(CommandLineErrorCode.Unknown, "dummy message")]
        [InlineData(CommandLineErrorCode.ActionArgInitFailed, null)]
        [InlineData(CommandLineErrorCode.CommandLineArgsParseFailed, "")]
        [InlineData(CommandLineErrorCode.InvalidActionMethodDefinition, "   ")]
        [InlineData(
            CommandLineErrorCode.InvalidCategoryDefinition,
            "Dummy message format '{0}' '{1}'",
            "dummy value 1",
            "dummy value 2")]
        public void ConstructorGivenErrorCodeAndMessageFormatSuccessTest(
            CommandLineErrorCode errorCode,
            string messageFormat,
            params object[] args)
        {
            string expectedErrorMessage =
                string.IsNullOrWhiteSpace(messageFormat)
                ? ExceptionMessages.DefaultCommandLineExceptionMessage
                    .FormatInvariant(errorCode)
                : messageFormat.FormatInvariant(args);

            this.AssertActualValue(
                () => new CommandLineException(errorCode, messageFormat, args),
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.IsAssignableFrom<Exception>(actualValue);
                    Assert.Equal(errorCode, actualValue.ErrorCode);
                    Assert.Equal(expectedErrorMessage, actualValue.Message);
                });
        }

        [Theory]
        [InlineData(CommandLineErrorCode.Unknown)]
        [InlineData(CommandLineErrorCode.ActionArgInitFailed)]
        [InlineData(CommandLineErrorCode.CommandLineArgsParseFailed)]
        [InlineData(CommandLineErrorCode.InvalidActionMethodDefinition)]
        [InlineData(CommandLineErrorCode.InvalidCategoryDefinition)]
        public void SerializeConstructorSuccessTest(
            CommandLineErrorCode errorCode)
        {
            string expectedErrorMessage = @"Dummy message.";

            this.AssertActualValue(
                () =>
                {
                    CommandLineException exception =
                        new CommandLineException(
                            errorCode,
                            expectedErrorMessage);

                    using (var memoryStream = new MemoryStream())
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(memoryStream, exception);

                        memoryStream.Position = 0;
                        return formatter.Deserialize(memoryStream)
                            as CommandLineException;
                    }
                },
                actualValue =>
                {
                    Assert.NotNull(actualValue);
                    Assert.IsAssignableFrom<Exception>(actualValue);
                    Assert.Equal(errorCode, actualValue.ErrorCode);
                    Assert.Equal(expectedErrorMessage, actualValue.Message);
                });
        }
    }
}