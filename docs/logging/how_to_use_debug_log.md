# How to use debug log in .Net Core

## Steps

> the debug log demo only works in debugging mode.

1. Add Nuget package `Microsoft.Extensions.Logging.Debug` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/logging_demo/logging_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Logging.Debug
    dotnet restore
    ```

2. Add `Microsoft.Extensions.Logging` namespace.

    > e.g. [DebugLogDemo.cs](../../demos/logging_demo/DebugLogDemo.cs)
    ```csharp
    using Microsoft.Extensions.Logging;
    ```

3. Create logger factory.

    > e.g. [DebugLogDemo.cs](../../demos/logging_demo/DebugLogDemo.cs)

    * Create default logger factory, using [AddDebug()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.debugloggerfactoryextensions.adddebug?view=aspnetcore-2.0#Microsoft_Extensions_Logging_DebugLoggerFactoryExtensions_AddDebug_Microsoft_Extensions_Logging_ILoggerFactory_) method.
        > the default log level is Information.
        ```csharp
        ILoggerFactory defaultLoggerFactory = new LoggerFactory();
        defaultLoggerFactory.AddDebug();
        ```

    * Create runtime logger factory, using [AddDebug(Func<String,LogLevel,Boolean>)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.debugloggerfactoryextensions.adddebug?view=aspnetcore-2.0#Microsoft_Extensions_Logging_DebugLoggerFactoryExtensions_AddDebug_Microsoft_Extensions_Logging_ILoggerFactory_System_Func_System_String_Microsoft_Extensions_Logging_LogLevel_System_Boolean__) method.
        > coud define custom logic in filter function
        ```csharp
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
        ```

    > debug log not support config

4. Create logger instance via [CreateLogger(loggerName)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory.createlogger?view=aspnetcore-2.0#Microsoft_Extensions_Logging_ILoggerFactory_CreateLogger_System_String_) or [CreateLogger&lt;T&gt;()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerfactoryextensions.createlogger?view=aspnetcore-2.0#Microsoft_Extensions_Logging_LoggerFactoryExtensions_CreateLogger__1_Microsoft_Extensions_Logging_ILoggerFactory_) method.
    > e.g. [DebugLogDemo.cs](../../demos/logging_demo/DebugLogDemo.cs)
    ```csharp
    ILogger defaultLogger = factory.CreateLogger("DefaultLogger");
    ILogger demoLogger = factory.CreateLogger<DebugLogDemo>();
    ```

5. Write log message via [LoggerExtensions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerextensions?view=aspnetcore-2.0).

    > e.g. [DebugLogDemo.cs](../../demos/logging_demo/DebugLogDemo.cs)

    ```csharp
    string loggerName = "{logger_name}";
    EventId eventId = new EventId(1002, "DebugLogDemoEvent");
    logger.LogTrace(eventId, "LogTrace from {LOGGER}", loggerName);
    logger.LogDebug(eventId, "LogDebug from {LOGGER}", loggerName);
    logger.LogInformation(eventId, "LogInformation from {LOGGER}", loggerName);
    logger.LogWarning(eventId, "LogWarning from {LOGGER}", loggerName);
    logger.LogError(eventId, "LogError from {LOGGER}", loggerName);
    logger.LogCritical(eventId, "LogCritical from {LOGGER}", loggerName);
    ```

    > debug log not support scopes

6. Optionally, you could check log level is enabled or not before writing log messages.

    > e.g. [DebugLogDemo.cs](../../demos/logging_demo/DebugLogDemo.cs)

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
* [Microsoft.Extensions.Logging.Debug Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.debug)
* [Microsoft.Extensions.Logging (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Logging)
* [Microsoft.Extensions.Logging.Debug (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Debug)
* [Microsoft.Extensions.Logging (github.com)](https://github.com/aspnet/Logging/tree/dev/src/Microsoft.Extensions.Logging)
* [Microsoft.Extensions.Logging.Debug (github.com)](https://github.com/aspnet/Logging/tree/dev/src/Microsoft.Extensions.Logging.Debug)
