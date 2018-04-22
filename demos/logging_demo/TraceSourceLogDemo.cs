/******************************************************************************
 * Copyright @ Pengzhi Sun 2018, all rights reserved.
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.
 *
 * File Name:   TraceSourceLogDemo.cs
 * Author:      Pengzhi Sun
 * Description: .Net Core trace source logging demos.
 * Reference:   https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging
 *              https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.tracesource
 *              https://github.com/aspnet/Logging
 *              https://github.com/aspnet/Logging/wiki/Guidelines
 *              https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.TraceSource/TraceSourceLoggerProvider.cs
 *              https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.TraceSource/TraceSourceLogger.cs
 *              https://msdn.microsoft.com/en-us/library/system.diagnostics.textwritertracelistener(v=vs.110).aspx
 *****************************************************************************/

namespace DotNetCoreBootstrap.LoggingDemo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the trace source log demo class.
    /// </summary>
    /// <remarks>
    /// Depends on Nuget packages:
    /// Microsoft.Extensions.Logging.TraceSource
    /// </remarks>
    internal static class TraceSourceLogDemo
    {
        /// <summary>
        /// Run the demo.
        /// </summary>
        public static void Run()
        {
            // in debugging mode, the default trace listner write logs to debug console.
            Console.WriteLine($"Is debugging: {Debugger.IsAttached}");
            Console.WriteLine();

            // create file based logger factory
            string logFilePath =
                Path.Combine(AppContext.BaseDirectory, "TraceSourceLogDemo.log");
            Stream logStream = File.Create(logFilePath);
            TextWriterTraceListener fileListener =
                new TextWriterTraceListener(logStream);

            // Information, Warning, Error, Critical, Verbose (Trace/Debug)
            SourceSwitch fileSwitch = new SourceSwitch("FileSwtich", "Verbose");
            ILoggerFactory fileLoggerFactory = new LoggerFactory();
            fileLoggerFactory.AddTraceSource(fileSwitch, fileListener);

            // create console based logger factory
            TextWriterTraceListener consoleListener =
                new TextWriterTraceListener(Console.Out);
            SourceSwitch consoleSwitch = new SourceSwitch("ConsoleSwitch", "Warning");
            ILoggerFactory consoleLoggerFactory = new LoggerFactory();
            consoleLoggerFactory.AddTraceSource(consoleSwitch, consoleListener);

            EventId eventId = new EventId(1003, "TraceSourceLogDemoEvent");

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

                    // file and console linster not support scopes
                    using (logger.BeginScope("[{LOGGER}]TraceSourceLogDemoScope", loggerName))
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

            // console log level: >= Warning,
            loggerDemoAction(consoleLoggerFactory, "ConsoleLogger");

            // file log level: >= Verbose,
            loggerDemoAction(fileLoggerFactory, "FileLogger");

            // dispose file logger factory to release the log file handler.
            fileLoggerFactory.Dispose();

            // print log file.
            Console.WriteLine($"[Trace] log file path: {logFilePath}");
            Console.WriteLine(File.ReadAllText(logFilePath, Encoding.UTF8));
        }
    }
}