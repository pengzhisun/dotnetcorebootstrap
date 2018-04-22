# How to use trace soruce log in .Net Core

## Steps

> in debugging mode, the default trace listener write logs to debug console.

1. Add Nuget package `Microsoft.Extensions.Logging.TraceSource` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/logging_demo/logging_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Logging.TraceSource
    dotnet restore
    ```

2. Add `Microsoft.Extensions.Logging` namespace.

    > e.g. [TraceSourceLogDemo.cs](../../demos/logging_demo/TraceSourceLogDemo.cs)
    ```csharp
    using Microsoft.Extensions.Logging;
    ```

3. Create logger factory.

    > e.g. [TraceSourceLogDemo.cs](../../demos/logging_demo/TraceSourceLogDemo.cs)

    * Create console logger factory, using [AddTraceSource(SourceSwitch, TraceListener)](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.logging.tracesourcefactoryextensions.addtracesource?view=aspnetcore-2.0#Microsoft_Extensions_Logging_TraceSourceFactoryExtensions_AddTraceSource_Microsoft_Extensions_Logging_ILoggerFactory_System_Diagnostics_SourceSwitch_System_Diagnostics_TraceListener_) method.
        > using Console.Out as [TextWriterTraceListener](https://msdn.microsoft.com/en-us/library/system.diagnostics.textwritertracelistener(v=vs.110).aspx) output stream.
        ```csharp
        TextWriterTraceListener consoleListener =
            new TextWriterTraceListener(Console.Out);
        SourceSwitch consoleSwitch = new SourceSwitch("ConsoleSwitch", "Warning");
        ILoggerFactory consoleLoggerFactory = new LoggerFactory();
        consoleLoggerFactory.AddTraceSource(consoleSwitch, consoleListener);
        ```

        > Supported switch value:

        * Verbose (for Debug&Trace log level)
        * Information
        * Warning
        * Error
        * Critical

    * Create file logger factory, using [AddTraceSource(SourceSwitch, TraceListener)](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.logging.tracesourcefactoryextensions.addtracesource?view=aspnetcore-2.0#Microsoft_Extensions_Logging_TraceSourceFactoryExtensions_AddTraceSource_Microsoft_Extensions_Logging_ILoggerFactory_System_Diagnostics_SourceSwitch_System_Diagnostics_TraceListener_) method.
        > using file stream as [TextWriterTraceListener](https://msdn.microsoft.com/en-us/library/system.diagnostics.textwritertracelistener) output stream.
        ```csharp
        string logFilePath =
            Path.Combine(AppContext.BaseDirectory, "TraceSourceLogDemo.log");
        Stream logStream = File.Create(logFilePath);
        TextWriterTraceListener fileListener =
            new TextWriterTraceListener(logStream);
        SourceSwitch fileSwitch = new SourceSwitch("FileSwtich", "Verbose");
        ILoggerFactory fileLoggerFactory = new LoggerFactory();
        fileLoggerFactory.AddTraceSource(fileSwitch, fileListener);
        ```

        > **MUST** displose the fileLoggerFactory to release the log file handler
        ```csharp
        fileLoggerFactory.Dispose();
        ```

4. Create logger instance via [CreateLogger(loggerName)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory.createlogger?view=aspnetcore-2.0#Microsoft_Extensions_Logging_ILoggerFactory_CreateLogger_System_String_) or [CreateLogger&lt;T&gt;()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerfactoryextensions.createlogger?view=aspnetcore-2.0#Microsoft_Extensions_Logging_LoggerFactoryExtensions_CreateLogger__1_Microsoft_Extensions_Logging_ILoggerFactory_) method.
    > e.g. [TraceSourceLogDemo.cs](../../demos/logging_demo/TraceSourceLogDemo.cs)
    ```csharp
    ILogger defaultLogger = factory.CreateLogger("DefaultLogger");
    ILogger demoLogger = factory.CreateLogger<TraceSourceLogDemo>();
    ```

5. Write log message via [LoggerExtensions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerextensions?view=aspnetcore-2.0).

    > e.g. [TraceSourceLogDemo.cs](../../demos/logging_demo/TraceSourceLogDemo.cs)

    ```csharp
    string loggerName = "{logger_name}";
    EventId eventId = new EventId(1003, "TraceSourceLogDemoEvent");
    logger.LogTrace(eventId, "LogTrace from {LOGGER}", loggerName);
    logger.LogDebug(eventId, "LogDebug from {LOGGER}", loggerName);
    logger.LogInformation(eventId, "LogInformation from {LOGGER}", loggerName);
    logger.LogWarning(eventId, "LogWarning from {LOGGER}", loggerName);
    logger.LogError(eventId, "LogError from {LOGGER}", loggerName);
    logger.LogCritical(eventId, "LogCritical from {LOGGER}", loggerName);
    ```

    > console and file based trace source log not support scopes

6. Optionally, you could check log level is enabled or not before writing log messages.

    > e.g. [TraceSourceLogDemo.cs](../../demos/logging_demo/TraceSourceLogDemo.cs)

    ```csharp
    IEnumerable<LogLevel> logLevels = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().Except(new[] { LogLevel.None });
    foreach (var logLevel in logLevels)
    {
        bool isEnabled = logger.IsEnabled(logLevel);
        Console.WriteLine($"\t[{(int)logLevel}]{logLevel} is enabled: ".PadRight(30, ' ') + isEnabled);
    }
    ```

## References

* [Logging in ASP.NET Core (docs.microsoft.com)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/)
* [Logging Guidelines (github.com)](https://github.com/aspnet/Logging/wiki/Guidelines)
* [Microsoft.Extensions.Logging Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging)
* [Microsoft.Extensions.Logging.TraceSource Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.tracesource)
* [Microsoft.Extensions.Logging (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Logging)
* [Microsoft.Extensions.Logging.TraceSource (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Logging.TraceSource)
* [Microsoft.Extensions.Logging (github.com)](https://github.com/aspnet/Logging/tree/dev/src/Microsoft.Extensions.Logging)
* [Microsoft.Extensions.Logging.TraceSource (github.com)](https://github.com/aspnet/Logging/tree/dev/src/Microsoft.Extensions.Logging.TraceSource)
* [TextWriterTraceListener Class (msdn.microsoft.com)](https://msdn.microsoft.com/en-us/library/system.diagnostics.textwritertracelistener)