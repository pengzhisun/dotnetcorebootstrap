/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   DebugLogDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core debug logging demos.
 * Reference:   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.debug
 *              https://github.com/aspnet/Logging
 *              https://github.com/aspnet/Logging/wiki/Guidelines
 *              https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Debug/DebugLogger.cs
 *****************************************************************************/

namespace DotNetCoreBootstrap.LoggingDemo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the debug log demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages:
    /// Microsoft.Extensions.Logging.Debug
    /// </remarks>
    internal sealed class DebugLogDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            // this demo only works in debugging mode
            Console.WriteLine($"Is debugging: {Debugger.IsAttached}");
            Console.WriteLine();

            ILoggerFactory defaultLoggerFactory = new LoggerFactory();
            defaultLoggerFactory.AddDebug();

            // debug log only support runtime factory, not support config
            ILoggerFactory runtimeLoggerFactory = new LoggerFactory();
            runtimeLoggerFactory
                .AddDebug(
                    (name, logLevel) =>
                    {
                        if (name == "RuntimeLogger")
                        {
                            switch (logLevel)
                            {
                                case LogLevel.Information:
                                case LogLevel.Error:
                                    return true;
                            }
                        }

                        return false;
                    });

            EventId eventId = new EventId(1002, "DebugLogDemoEvent");

            IEnumerable<LogLevel> logLevels =
                Enum.GetValues(typeof(LogLevel))
                    .Cast<LogLevel>()
                    .Except(new[] { LogLevel.None });

            Action<ILoggerFactory, string> loggerDemoAction =
                (factory, loggerName) =>
                {
                    Console.WriteLine($"Logger name: {loggerName}");

                    // also support factory.CreateLogger<T>();
                    ILogger logger = factory.CreateLogger(loggerName);

                    foreach (var logLevel in logLevels)
                    {
                        bool isEnabled = logger.IsEnabled(logLevel);
                        Console.WriteLine($"\t[{(int)logLevel}]{logLevel} is enabled: ".PadRight(30, ' ') + isEnabled);
                    }

                    // debug log not support scopes
                    using (logger.BeginScope("[{LOGGER}]LogDemoScope", loggerName))
                    {
                        logger.LogTrace(eventId, "LogTrace from {LOGGER}", loggerName);
                        logger.LogDebug(eventId, "LogDebug from {LOGGER}", loggerName);
                        logger.LogInformation(eventId, "LogInformation from {LOGGER}", loggerName);
                        logger.LogWarning(eventId, "LogWarning from {LOGGER}", loggerName);
                        logger.LogError(eventId, "LogError from {LOGGER}", loggerName);
                        logger.LogCritical(eventId, "LogCritical from {LOGGER}", loggerName);
                    }

                    Console.WriteLine();
                };

            // default log level: >= Information
            loggerDemoAction(defaultLoggerFactory, "DefaultLogger");

            // runtime log level: Information and Error
            loggerDemoAction(runtimeLoggerFactory, "RuntimeLogger");
        }
    }
}