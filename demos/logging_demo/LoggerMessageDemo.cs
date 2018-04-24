/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   LoggerMessageDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core logger message demos.
 * Reference:   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/
 *              https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.console
 *              https://github.com/aspnet/Logging
 *              https://github.com/aspnet/Logging/wiki/Guidelines
 *              https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Abstractions/LoggerMessage.cs
 *              https://github.com/aspnet/Logging/blob/dev/samples/SampleApp/LoggerExtensions.cs
 *              https://github.com/aspnet/Logging/blob/dev/samples/SampleApp/Program.cs
 *****************************************************************************/

namespace DotNetCoreBootstrap.LoggingDemo
{
    using System;
    using System.Threading;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the logger message demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages:
    /// Microsoft.Extensions.Logging.Console
    /// </remarks>
    internal sealed class LoggerMessageDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            // use runtime config logger with includScopes enabled for demo.
            ILoggerFactory loggerFactory = new LoggerFactory();
                loggerFactory
                    .AddConsole(
                        minLevel: LogLevel.Trace,
                        includeScopes: true);

            ILogger logger =
                loggerFactory.CreateLogger<LoggerMessageDemo>();

            // define scope method accept 0~3 type parameters
            Func<ILogger, string, IDisposable> defineScopeFunc =
                LoggerMessage.DefineScope<string>("LoggerScope: {ScopeName}");

            EventId eventId = new EventId(1004, "LoggerMessageDemoEvent");

            // define method accept 0~6 type parameters
            Action<ILogger, string, int, Exception> logAction =
                LoggerMessage.Define<string, int>(
                    LogLevel.Information,
                    eventId,
                    "Log Message: stringValue = '{StringValue}', intValue = '{IntValue}'");

            using (defineScopeFunc(logger, "DemoScope"))
            {
                logAction(logger, "string_value_1", 1, null);
            }

            // flush the backgroud console thread
            Thread.Sleep(TimeSpan.FromMilliseconds(10));
        }
    }
}