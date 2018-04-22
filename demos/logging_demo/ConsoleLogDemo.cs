/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   ConsoleLogDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core console logging demos.
 * Reference:   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.console
 *              https://github.com/aspnet/Logging
 *              https://github.com/aspnet/Logging/wiki/Guidelines
 *              https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Abstractions/LogLevel.cs
 *              https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Console/ConsoleLoggerFactoryExtensions.cs
 *              https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Console/ConfigurationConsoleLoggerSettings.cs
 *              https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Console/ConsoleLoggerProvider.cs
 *              https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Console/ConsoleLogger.cs
 *              https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Console/Internal/ConsoleLoggerProcessor.cs
 *****************************************************************************/

namespace DotNetCoreBootstrap.LoggingDemo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the console log demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages:
    /// Microsoft.Extensions.Configuration.Binder
    /// Microsoft.Extensions.Configuration.Json
    /// Microsoft.Extensions.Logging.Console
    /// </remarks>
    internal static class ConsoleLogDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("ConsoleLogDemoConfig.json",
                optional: true,
                reloadOnChange: true);

            IConfiguration config = configBuilder.Build();

            IConfiguration consoleConfig =
                config.GetSection(("Logging:Console"));

            ILoggerFactory defaultLoggerFactory = new LoggerFactory();
            defaultLoggerFactory.AddConsole();

            ILoggerFactory runtimeLoggerFactory = new LoggerFactory();
            runtimeLoggerFactory
                .AddConsole(
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
                    },
                    includeScopes: false);

            ILoggerFactory configLoggerFactory = new LoggerFactory();
            configLoggerFactory.AddConsole(consoleConfig);

            EventId eventId = new EventId(1001, "ConsoleLogDemoEvent");

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

                    using (logger.BeginScope("[{LOGGER}]LogDemoScope", loggerName))
                    {
                        logger.LogTrace(eventId, "LogTrace from {LOGGER}", loggerName);
                        logger.LogDebug(eventId, "LogDebug from {LOGGER}", loggerName);
                        logger.LogInformation(eventId, "LogInformation from {LOGGER}", loggerName);
                        logger.LogWarning(eventId, "LogWarning from {LOGGER}", loggerName);
                        logger.LogError(eventId, "LogError from {LOGGER}", loggerName);
                        logger.LogCritical(eventId, "LogCritical from {LOGGER}", loggerName);
                    }

                    // flush the backgroud console thread
                    Thread.Sleep(TimeSpan.FromMilliseconds(10));

                    Console.WriteLine();
                };

            // default log level: >= Information, includeScopes: false
            loggerDemoAction(defaultLoggerFactory, "DefaultLogger");

            // runtime log level: Information and Error, includeScopes: false
            loggerDemoAction(runtimeLoggerFactory, "RuntimeLogger");

            // "DotNetCoreBootstrap" >= "Trace"
            loggerDemoAction(configLoggerFactory, "DotNetCoreBootstrap.LoggingDemo");

            // "Microsoft.Extensions" >= "Error", ingore the "Logging" config
            loggerDemoAction(configLoggerFactory, "Microsoft.Extensions.Logging");

            // "Microsoft.Extensions.Config" >= "Information"
            loggerDemoAction(configLoggerFactory, "Microsoft.Extensions.Config");

            // the search path from config:
            // "System.Threading.Tasks", not found
            // "System.Threading", not found
            // "System", not found
            // "Default" = "Default" >= "Critical"
            // LogLevel.None, default value if "Default" not found
            loggerDemoAction(configLoggerFactory, "System.Threading.Tasks");

            // print config file:
            string configFilePath =
                Path.Combine(AppContext.BaseDirectory, "ConsoleLogDemoConfig.json");
            Console.WriteLine($"[Trace] config file path: {configFilePath}");
            string configFileContent =
                File.ReadAllText(configFilePath, Encoding.UTF8);
            Console.WriteLine(configFileContent);
        }
    }
}