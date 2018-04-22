# How to use console log in .Net Core

## Steps

1. Add Nuget package `Microsoft.Extensions.Logging.Console` reference via `dotnet add {project} package {package}` command.

    > `{project}` sample: demos/logging_demo/logging_demo.csproj

    ```bash
    dotnet add {project} package Microsoft.Extensions.Logging.Console
    dotnet restore
    ```

2. Add logging configuration file.

   > e.g. [ConsoleLogDemoConfig.json](../../demos/logging_demo/ConsoleLogDemoConfig.json), refer this guide: [How to use a JSON format configuration file in .Net Core](../config/how_to_use_json_config_file.md)
    ```json
    {
        "Logging": {
            "Console": {
                "IncludeScopes": "true",
                "LogLevel": {
                    "Default": "Critical",
                    "DotNetCoreBootstrap": "Trace",
                    "Microsoft.Extensions": "Error",
                    "Logging": "Warninig",
                    "Microsoft.Extensions.Config": "Information"
                }
            }
        }
    }
    ```
    > the logger will filter the log level by following path:
    * "System.Threading.Tasks.Task"
    * "System.Threading.Tasks"
    * "System.Threading"
    * "System"
    * "Default", the default switch, e.g. "Default": "Critical"
    * LogLevel.None, if no swtich found

3. Add `Microsoft.Extensions.Logging` namespace.

    > e.g. [ConsoleLogDemo.cs](../../demos/logging_demo/ConsoleLogDemo.cs)
    ```csharp
    using Microsoft.Extensions.Logging;
    ```

4. Create logger factory.

    > e.g. [ConsoleLogDemo.cs](../../demos/logging_demo/ConsoleLogDemo.cs)

    * Create default logger factory, using [AddConsole()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.consoleloggerextensions.addconsole?view=aspnetcore-2.0#Microsoft_Extensions_Logging_ConsoleLoggerExtensions_AddConsole_Microsoft_Extensions_Logging_ILoggerFactory_) method.
        > the default log level is Information, and the includeScopes value is false.
        ```csharp
        ILoggerFactory defaultLoggerFactory = new LoggerFactory();
        defaultLoggerFactory.AddConsole();
        ```

    * Create runtime logger factory, using [AddConsole(Func<String,LogLevel,Boolean>)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.consoleloggerextensions.addconsole?view=aspnetcore-2.0#Microsoft_Extensions_Logging_ConsoleLoggerExtensions_AddConsole_Microsoft_Extensions_Logging_ILoggerFactory_System_Func_System_String_Microsoft_Extensions_Logging_LogLevel_System_Boolean__System_Boolean_) method.
        > coud define custom logic in filter function
        ```csharp
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
        ```

    * Create logger factory based on config file, using [AddConsole(IConsoleLoggerSettings)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.consoleloggerextensions.addconsole?view=aspnetcore-2.0#Microsoft_Extensions_Logging_ConsoleLoggerExtensions_AddConsole_Microsoft_Extensions_Logging_ILoggerFactory_Microsoft_Extensions_Logging_Console_IConsoleLoggerSettings_) method.
        > refer this guide: [How to use a JSON format configuration file in .Net Core](../config/how_to_use_json_config_file.md)
        ```csharp
        IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("ConsoleLogDemoConfig.json",
                optional: true,
                reloadOnChange: true);

        IConfiguration config = configBuilder.Build();

        IConfiguration consoleConfig = config.GetSection(("Logging:Console"));

        ILoggerFactory configLoggerFactory = new LoggerFactory();
            configLoggerFactory.AddConsole(consoleConfig);
        ```

5. Create logger instance via [CreateLogger(loggerName)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory.createlogger?view=aspnetcore-2.0#Microsoft_Extensions_Logging_ILoggerFactory_CreateLogger_System_String_) or [CreateLogger&lt;T&gt;()](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerfactoryextensions.createlogger?view=aspnetcore-2.0#Microsoft_Extensions_Logging_LoggerFactoryExtensions_CreateLogger__1_Microsoft_Extensions_Logging_ILoggerFactory_) method.
    > e.g. [ConsoleLogDemo.cs](../../demos/logging_demo/ConsoleLogDemo.cs)
    ```csharp
    ILogger defaultLogger = factory.CreateLogger("DefaultLogger");
    ILogger demoLogger = factory.CreateLogger<ConsoleLogDemo>();
    ```

6. Write log message via [LoggerExtensions](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerextensions?view=aspnetcore-2.0).

    > e.g. [ConsoleLogDemo.cs](../../demos/logging_demo/ConsoleLogDemo.cs)

    ```csharp
    string loggerName = "{logger_name}";
    EventId eventId = new EventId(1001, "ConsoleLogDemoEvent");
    logger.LogTrace(eventId, "LogTrace from {LOGGER}", loggerName);
    logger.LogDebug(eventId, "LogDebug from {LOGGER}", loggerName);
    logger.LogInformation(eventId, "LogInformation from {LOGGER}", loggerName);
    logger.LogWarning(eventId, "LogWarning from {LOGGER}", loggerName);
    logger.LogError(eventId, "LogError from {LOGGER}", loggerName);
    logger.LogCritical(eventId, "LogCritical from {LOGGER}", loggerName);
    ```

    > could use [BeginScope](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerextensions.beginscope?view=aspnetcore-2.0#Microsoft_Extensions_Logging_LoggerExtensions_BeginScope_Microsoft_Extensions_Logging_ILogger_System_String_System_Object___) block to group a set of log messages.
    ```csharp
    string loggerName = "{logger_name}";
    EventId eventId = new EventId(1001, "ConsoleLogDemoEvent");
    using (logger.BeginScope($"[{loggerName}]ConsoleLogDemoScope"))
    {
        logger.LogTrace(eventId, "LogTrace from {LOGGER}", loggerName);
        logger.LogDebug(eventId, "LogDebug from {LOGGER}", loggerName);
        logger.LogInformation(eventId, "LogInformation from {LOGGER}", loggerName);
        logger.LogWarning(eventId, "LogWarning from {LOGGER}", loggerName);
        logger.LogError(eventId, "LogError from {LOGGER}", loggerName);
        logger.LogCritical(eventId, "LogCritical from {LOGGER}", loggerName);
    }
    ```

    > the console log will be processed in backgroud thread, for demo purpose we need to sleep main thread a little.
    ```csharp
    Thread.Sleep(TimeSpan.FromMilliseconds(10));
    ```

7. Optionally, you could check log level is enabled or not before writing log messages.

    > e.g. [ConsoleLogDemo.cs](../../demos/logging_demo/ConsoleLogDemo.cs)
    
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
* [Microsoft.Extensions.Logging.Console Namespace (docs.microsoft.com)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.console)
* [Microsoft.Extensions.Logging (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Logging)
* [Microsoft.Extensions.Logging.Console (nuget.org)](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Console)
* [Microsoft.Extensions.Logging (github.com)](https://github.com/aspnet/Logging/tree/dev/src/Microsoft.Extensions.Logging)
* [Microsoft.Extensions.Logging.Console (github.com)](https://github.com/aspnet/Logging/tree/dev/src/Microsoft.Extensions.Logging.Console)
